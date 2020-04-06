using System.Collections;
using UnityEngine;

public class Dash : PlayerAbility 
{
    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        StartCoroutine(DashUpdate(direction.normalized));
    }

    private IEnumerator DashUpdate(Vector3 direction)
    {
        base.OnAbilityRequestEnd(direction);

        var startPosition = player.Position;
        while ((player.Position - startPosition).magnitude < Range)
        {
            player.Translate(direction * Time.deltaTime * Speed);
            yield return new WaitForFixedUpdate();
        }

    }

}
