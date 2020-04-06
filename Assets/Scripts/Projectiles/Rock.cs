using Photon.Pun;
using UnityEngine;

public class Rock : MonoBehaviourPun
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    public void Spawn(Vector3 spawnPosition)
    {
        photonView.RPC(nameof(SpawnRPC), RpcTarget.All, spawnPosition);
    }

    [PunRPC]
    public void SpawnRPC(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}
