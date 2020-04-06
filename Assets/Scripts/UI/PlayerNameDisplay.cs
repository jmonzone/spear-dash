using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDisplay : MonoBehaviour
{
    public void Init(Player player)
    {
        var text = GetComponent<Text>();
        text.text = player.name;
        Debug.Log("Initializing name display for " + player.name);

        StartCoroutine(FollowPlayer(player.transform));
    }

    private IEnumerator FollowPlayer(Transform player)
    {
        while (player.gameObject.activeSelf)
        {
            var position = Camera.main.WorldToScreenPoint(player.position) + Vector3.up * 250.0f;
            transform.position = position;
            yield return new WaitForFixedUpdate();
        }
    }
}
