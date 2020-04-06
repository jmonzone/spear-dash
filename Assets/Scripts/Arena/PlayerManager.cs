using System;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] private PlayerAbilityManager clientAbilityManager;
    [SerializeField] private PlayerMovementManager clientMovementManager;
    [SerializeField] private PlayerAnimator clientAnimator;

    private Player clientPlayer;

    public static event Action<Player> OnClientPlayerSpawned;
    public static event Action<Player> OnOtherPlayerSpawned;

    private void Start()
    {
        SpawnPlayer("Red Player");

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < GameManager.Instance.NumCompPlayers; i++)
            {
                SpawnComputer("Red Player");
            }
        }

    }

    private void SpawnPlayer(string characterPath)
    {
        clientPlayer = PhotonNetwork.Instantiate("Characters/" + characterPath, UnityEngine.Random.insideUnitCircle * 3.0f, Quaternion.identity, 0).GetComponent<Player>();
        photonView.RPC(nameof(OnPlayerSpawned), RpcTarget.OthersBuffered, clientPlayer.photonView.ViewID, PhotonNetwork.LocalPlayer.NickName);

        clientPlayer.name = PhotonNetwork.LocalPlayer.NickName;
        clientPlayer.gameObject.layer = 8;

        clientAbilityManager.Init(clientPlayer);
        clientMovementManager.Init(clientPlayer);
        clientAnimator.Init(clientPlayer);

        OnClientPlayerSpawned?.Invoke(clientPlayer);
    }

    private void SpawnComputer(string characterPath)
    {
        var player = PhotonNetwork.Instantiate("Characters/" + characterPath, UnityEngine.Random.insideUnitCircle * 3.0f, Quaternion.identity, 0).GetComponent<Player>();
        photonView.RPC(nameof(SpawnComputerRPC), RpcTarget.OthersBuffered, player.photonView.ViewID);

        player.name = "AI Player";
        player.gameObject.layer = 9;
        player.Init(true);

        var compControllerPrefab = Resources.Load<AIController>("AI Controller");
        var compController = Instantiate(compControllerPrefab);
        compController.Init(player, clientPlayer);

        OnOtherPlayerSpawned?.Invoke(player);

    }


    [PunRPC]
    public void OnPlayerSpawned(int viewId, string name)
    {
        var pv = PhotonView.Find(viewId);
        var player = pv.GetComponent<Player>();
        player.name = name;
        player.gameObject.layer = 9;
        OnOtherPlayerSpawned?.Invoke(player);
    }

    [PunRPC]
    public void SpawnComputerRPC(int viewId)
    {
        var pv = PhotonView.Find(viewId);
        var player = pv.GetComponent<Player>();
        player.name = "AI Player";
        player.gameObject.layer = 9;
        player.Init(true);

        OnOtherPlayerSpawned?.Invoke(player);
    }
}
