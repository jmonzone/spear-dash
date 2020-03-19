using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spriteTransform;

    private void Awake()
    {
        var anim = GetComponentInChildren<Animator>();

        var walk = GetComponent<PlayerWalk>();
        walk.OnWalkStart += () =>
        {
            anim.SetInteger("State", 2);
        };
        walk.OnWalkEnd += () =>
        {
            anim.SetInteger("State", 0);
        };
        walk.OnWalk += (direction) =>
        {
            FaceDirection(direction);
        };
    }

    private void FaceDirection(Vector3 direction)
    {
        var scale = spriteTransform.localScale;
        var sign = direction.x < 0 ? -1 : 1;
        scale.x = sign * Mathf.Abs(scale.x);
        spriteTransform.localScale = scale;
    }

}
