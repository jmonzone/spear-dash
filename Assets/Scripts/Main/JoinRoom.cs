using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button playVersusPlayerButton;
    [SerializeField] private Button playVersussAIButton;

    protected void Awake()
    {
        playVersusPlayerButton.onClick.AddListener(OnPlayVersusPlayerButtonClicked);
        playVersussAIButton.onClick.AddListener(OnPlayVersusAIButtonClicked);
        playVersusPlayerButton.interactable = playVersussAIButton.interactable = false;
    }

    private void OnDestroy()
    {
        playVersusPlayerButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        if(!PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.ConnectUsingSettings();
    }

    private void OnPlayVersusPlayerButtonClicked()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            EmptyRoomTtl = 0
        };

        PhotonNetwork.JoinOrCreateRoom("Test", roomOptions, TypedLobby.Default);
    }

    private void OnPlayVersusAIButtonClicked()
    {
        GameManager.Instance.NumCompPlayers = 1;
        OnPlayVersusPlayerButtonClicked();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("Connected to Master");
        playVersusPlayerButton.interactable = playVersussAIButton.interactable = true;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print($"Connected to room: {PhotonNetwork.CurrentRoom.Name}");

        PhotonNetwork.LoadLevel("PhotonScene");
    }
}