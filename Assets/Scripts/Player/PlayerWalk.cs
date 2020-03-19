using System;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    private const float WALK_SPEED = 1.5f;

    public event Action OnWalkStart;
    public event Action OnWalkEnd;
    public event Action<Vector3> OnWalk;

    private void Start()
    {
        var controller = GetComponent<IController>();
        controller.OnWalkRequestStart += () => OnWalkStart();
        controller.OnWalkRequestEnd += () => OnWalkEnd();
        controller.OnWalkRequest += (direction) => Walk(direction);
    }

    private void Walk(Vector3 direction)
    {
        direction.Normalize();
        transform.Translate(direction * Time.deltaTime * WALK_SPEED);
        OnWalk?.Invoke(direction);
    }
}
