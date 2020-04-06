using System.Collections.Generic;
using UnityEngine;

public class DirectionAbilityIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform arrow;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        var abilityManager = GetComponent<PlayerAbilityManager>();
        abilityManager.OnAbilitiesInit += OnAbilitiesInit;
    }

    private void OnAbilitiesInit(List<PlayerAbility> abilities)
    {
        abilities.ForEach((ability) =>
        {
            if (ability.TargetType == TargetingType.DIRECTION)
            {
                ability.OnAbilityStart += (startPosition) =>
                {
                    Display();
                };

                ability.OnAbilityAim += (args) =>
                {
                    var range = Mathf.Clamp(args.range, 1.5f, 5.0f);

                    lineRenderer.SetPosition(0, args.startPosition);
                    lineRenderer.SetPosition(1, args.startPosition + args.direction.normalized * range);

                    arrow.up = -args.direction;
                    arrow.position = args.startPosition + args.direction.normalized * range;
                };

                ability.OnAbilityEnd += (direction) =>
                {
                    Display(false);
                };

                ability.OnAbilityCancel += () =>
                {
                    Display(false);
                };
            }
        });
    }

    public void Display(bool display = true)
    {
        lineRenderer.enabled = display;
        arrow.gameObject.SetActive(display);
    }
}
