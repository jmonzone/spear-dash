using UnityEngine;

public class PlayerNameDisplayManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerManager.OnClientPlayerSpawned += InstantiateNameDisplay;
        PlayerManager.OnOtherPlayerSpawned += InstantiateNameDisplay;
    }

    private void InstantiateNameDisplay(Player player)
    {
        var prefab = Resources.Load<PlayerNameDisplay>("Player Name Display");
        var display = Instantiate(prefab, transform);
        display.Init(player);

    }
}
