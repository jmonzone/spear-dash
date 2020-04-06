using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Player player;
    private Player enemy;

    public void Init(Player player, Player enemy)
    {
        this.player = player;
        this.enemy = enemy;

        var movementManager = GetComponent<PlayerMovementManager>();
        movementManager.Init(player);
        StartCoroutine(WalkUpdate(movementManager));

        var abilityManager = GetComponent<PlayerAbilityManager>();
        abilityManager.OnAbilitiesInit += OnAbilitiesInit;
        abilityManager.Init(player);

        var animator = GetComponent<PlayerAnimator>();
        animator.Init(player);
    }

    private void OnAbilitiesInit(List<PlayerAbility> abilities)
    {
        abilities.ForEach((ability) =>
        {
            ability.OnCooldownEnd += (cd) =>
            {
                StartCoroutine(AttackUpdate(ability));
            };
        });
    }

    private IEnumerator WalkUpdate(PlayerMovementManager walk)
    {
        walk.OnWalkRequestStart();

        var size = ArenaManager.Instance.Size / 4.0f;
        var direction = Vector3.zero;

        while (gameObject.activeSelf)
        {
            var x = Random.Range(-size.x, size.x);
            var y = Random.Range(-size.y, size.y);
            var targetPosition = new Vector3(x, y);

            while(Vector3.Distance(player.Position, targetPosition) > 0.05f)
            {
                direction = (targetPosition - player.Position).normalized;
                walk.OnWalkRequest(direction);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }

        walk.OnWalkRequestEnd(direction);
    }

    private IEnumerator AttackUpdate(PlayerAbility selectedAbility)
    {
        var randomDelay = Random.Range(0.0f, 1.0f);
        yield return new WaitForSeconds(randomDelay);

        selectedAbility.OnAbilityRequestStart();

        Vector3 direction = Vector3.zero;

        switch (selectedAbility.AbilityType)
        {
            case AbilityType.DAMAGE:
                direction = enemy.Position - player.Position;
                break;
            case AbilityType.DASH:
                direction = Random.insideUnitCircle * selectedAbility.Range;
                break;
            case AbilityType.TRAP:
                var targetPosition = enemy.Position + (Vector3)Random.insideUnitCircle * 4.5f;
                direction = targetPosition - player.Position;
                break;
        }

        selectedAbility.OnAbilityRequest(direction);
        selectedAbility.OnAbilityRequestEnd(direction);
    }
   
}
