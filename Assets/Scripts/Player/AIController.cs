using System;
using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    [Header("References")]
    [SerializeField] private Transform enemy;

    private PlayerAbilitySwitcher abilitySwitcher;


    public event Action OnWalkRequestStart;
    public event Action OnWalkRequestEnd;
    public event Action<Vector3> OnWalkRequest;

    private void Start()
    {
        abilitySwitcher = GetComponent<PlayerAbilitySwitcher>();
        abilitySwitcher.AbilityPool.ForEach((ability) =>
        {
            ability.OnCooldownEnd += (cd) =>
            {
                StartCoroutine(AttackUpdate(ability));
            };
        });

        StartCoroutine(WalkUpdate());
    }

    private IEnumerator WalkUpdate()
    {
        OnWalkRequestStart?.Invoke();

        var size = ArenaManager.Instance.Size;
        var halfSize = size / 4.0f;

        while (gameObject.activeSelf)
        {
            var x = UnityEngine.Random.Range(-halfSize.x, halfSize.y);
            var y = UnityEngine.Random.Range(-halfSize.y, halfSize.y);
            var targetPosition = new Vector3(x, y);

            while(Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                var direction = (targetPosition - transform.position).normalized;
                OnWalkRequest?.Invoke(direction);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }

        OnWalkRequestEnd?.Invoke();
    }

    private IEnumerator AttackUpdate(PlayerAbility selectedAbility)
    {
        var randomDelay = UnityEngine.Random.Range(0.0f, 2.0f);
        yield return new WaitForSeconds(randomDelay);

        selectedAbility.OnAbilityRequestStart();

        Vector3 direction = Vector3.zero;

        switch (selectedAbility.AbilityType)
        {
            case AbilityType.DAMAGE:
                direction = enemy.position - transform.position;
                break;
            case AbilityType.DASH:
                direction = UnityEngine.Random.insideUnitCircle;
                break;
            case AbilityType.TRAP:
                var targetPosition = enemy.position + (Vector3)UnityEngine.Random.insideUnitCircle * 4.5f;
                direction = targetPosition - transform.position;
                break;
        }
        selectedAbility.OnAbilityRequest(direction);
        selectedAbility.OnAbilityRequestEnd(direction);
    }
   
}
