using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    private Rigidbody2D rb;
    private Collider2D col;

    public event Action<GameObject> OnCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollision?.Invoke(collision.gameObject);
        StopAllCoroutines();
    }

    public void Init(int layer)
    {
        photonView.RPC(nameof(InitRPC), RpcTarget.AllBuffered, layer);
    }

    [PunRPC]
    public void InitRPC(int layer)
    {
        gameObject.layer = layer;
        gameObject.SetActive(false);
    }

    public void Display(bool display = true)
    {
        photonView.RPC(nameof(DisplayRPC), RpcTarget.All, display);
    }

    [PunRPC]
    public void DisplayRPC(bool display = true)
    {
        gameObject.SetActive(display);
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

    public void Shoot(Vector3 velocity, float range, Action<GameObject> onCollision = null, Action onTargetPositionReached = null)
    {
        rb.velocity = velocity;
        OnCollision = onCollision;
        col.enabled = true;

        StartCoroutine(RangeUpdate(range, onTargetPositionReached));
    }

    private IEnumerator RangeUpdate(float range, Action onComplete = null)
    {
        var start = transform.position;
        yield return new WaitUntil(() => (transform.position - start).magnitude > range);
        rb.velocity = Vector3.zero;
        onComplete?.Invoke();
    }
}
