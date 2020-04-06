using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Awake()
    {
        PlayerManager.OnClientPlayerSpawned += OnClientPlayerSpawned;
    }

    private void OnClientPlayerSpawned(Player  player)
    {
        StartCoroutine(FollowPlayer(player.transform));
    }

    private IEnumerator FollowPlayer(Transform player)
    {
        Debug.Log($"Camera is now following {player}");
        var offset = transform.position;
        while (player.gameObject.activeSelf)
        {
            transform.position = player.position + offset;
            yield return new WaitForFixedUpdate();
        }
    }
}
