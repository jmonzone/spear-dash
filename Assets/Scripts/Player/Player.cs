using System;
using Photon.Pun;
using UnityEngine;

public class Player  : MonoBehaviourPun
{
    public bool IsComputer { get; private set; } = false;

    public Vector3 Position => transform.position;
    public bool IsMine => photonView.IsMine;

    private Rigidbody2D rb;

    public event Action IsOnScreen;
    public event Action IsOffScreen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(bool isComputer)
    {
        IsComputer = isComputer;
    }

    private void OnBecameVisible()
    {
        Debug.Log($"{name} has become visible.");
        IsOnScreen?.Invoke();
    }

    private void OnBecameInvisible()
    {
        Debug.Log($"{name} has become invisible.");
        IsOffScreen?.Invoke();
    }

    public void Translate(Vector3 direction)
    {
        rb.MovePosition(Position + direction);
    }

    public void Knockback(Vector3 force)
    {
        rb.AddForce(force);
    }

    public void Teleport(Vector3 position)
    {
        photonView.RPC(nameof(TeleporRPC), RpcTarget.All, position);
    }

    [PunRPC]
    public void TeleporRPC(Vector3 position)
    {
        rb.MovePosition(position);
    }
}
