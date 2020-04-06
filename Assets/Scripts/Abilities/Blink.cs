using System.Collections;
using UnityEngine;

public class Blink : PlayerAbility 
{
    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        base.OnAbilityRequestEnd(direction);
        particle.gameObject.transform.position = player.Position;

        var targetPos = player.Position + Vector3.ClampMagnitude(direction, Range);
        player.Teleport(targetPos);
        particle.Play();
    }
}
