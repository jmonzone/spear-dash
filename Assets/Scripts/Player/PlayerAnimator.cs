using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Transform sprite;
    private Animator animator;

    public void Init(Player player)
    {
        animator = player.GetComponentInChildren<Animator>();
        sprite = player.transform.Find("Sprite");

        var walk = GetComponent<PlayerMovementManager>();

        walk.OnWalkStart += () =>
        {
            animator.SetInteger("State", 2);
        };

        walk.OnWalk += (direction) =>
        {
            FaceDirection(direction);
        };

        walk.OnWalkEnd += (direction) =>
        {
            animator.SetInteger("State", 0);
        };

        walk.OnWalkCancel += () =>
        {
            animator.SetInteger("State", 0);
        };
    }

    private void FaceDirection(Vector3 direction)
    {
        var scale = sprite.localScale;
        var sign = direction.x < 0 ? -1 : 1;
        scale.x = sign * Mathf.Abs(scale.x);
        sprite.localScale = scale;
    }

}
