using System.Collections.Generic;
using UnityEngine;

public class GroundAbilityIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform indicator;

    private void Awake()
    {
        var abilityManager = GetComponent<PlayerAbilityManager>();
        abilityManager.OnAbilitiesInit += OnAbilitiesInit;
    }

    private void OnAbilitiesInit(List<PlayerAbility> abilities)
    {
        abilities.ForEach((ability) =>
        {
            if (ability.TargetType == TargetingType.GROUND)
            {
                ability.OnAbilityStart += (startPosition) =>
                {
                    indicator.position = startPosition;
                    Display();
                };

                ability.OnAbilityAim += (args) =>
                {
                    var direction = Vector3.ClampMagnitude(args.direction, ability.Range);
                    indicator.position = args.startPosition + direction;
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
        indicator.gameObject.SetActive(display);
    }


}
