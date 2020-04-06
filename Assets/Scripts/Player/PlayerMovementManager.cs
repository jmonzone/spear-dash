using System;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    private const float WALK_SPEED = 2.0f;

    public event Action OnWalkStart;
    public event Action<Vector3> OnWalk;
    public event Action<Vector3> OnWalkEnd;
    public event Action OnWalkCancel;

    private Player player;

    public void Init(Player player)
    {
        this.player = player;
    }

    public virtual void OnWalkRequestStart()
    {
        OnWalkStart?.Invoke();
    }

    public virtual void OnWalkRequest(Vector3 direction)
    {
        direction.Normalize();
        player.Translate(direction * Time.deltaTime * WALK_SPEED);
        OnWalk?.Invoke(direction); OnWalk?.Invoke(direction);
    }

    public virtual void OnWalkRequestEnd(Vector3 direction)
    {
        OnWalkEnd?.Invoke(direction);
    }

    public virtual void OnWalkRequestCancel()
    {
        OnWalkCancel?.Invoke();
    }
}
