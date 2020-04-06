using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    private void Awake()
    {
        var playerName = PlayerPrefs.GetString(PlayerPrefsKeys.PreferredUsernamePrefsKey, "New Player");
        PhotonNetwork.LocalPlayer.NickName = playerName;

        var playerNameInputField = GetComponent<InputField>();
        playerNameInputField.text = playerName;

        playerNameInputField.onValueChanged.AddListener((name) =>
        {
            PlayerPrefs.SetString(PlayerPrefsKeys.PreferredUsernamePrefsKey, name);
            PhotonNetwork.LocalPlayer.NickName = name;
        });
    }
}
