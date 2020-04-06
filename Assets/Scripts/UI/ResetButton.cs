using Photon.Pun;
using UnityEngine.UI;

public class ResetButton : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveRoom();
        });
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
