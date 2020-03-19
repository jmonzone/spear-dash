using UnityEngine;

public class PlayerAttackDirectionIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform arrow;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        foreach (PlayerAbility ability in GetComponents<PlayerAbility>())
        {
            if (ability.TargetType == TargetingType.DIRECTION)
            {
                ability.OnAbilityStart += () =>
                {
                    lineRenderer.enabled = true;
                    arrow.gameObject.SetActive(true);
                };

                ability.OnAbilityAim += (args) =>
                {
                    var range = Mathf.Clamp(args.range, 1.5f, 5.0f);
                    lineRenderer.SetPosition(0, shootPosition.position);
                    lineRenderer.SetPosition(1, shootPosition.position + args.direction.normalized * range);
                    arrow.up = -args.direction;
                    arrow.position = shootPosition.position + args.direction.normalized * range;

                };

                ability.OnAbilityEnd += (direction) =>
                {
                    lineRenderer.enabled = false;
                    arrow.gameObject.SetActive(false);

                };
            }
        }
    }
}
