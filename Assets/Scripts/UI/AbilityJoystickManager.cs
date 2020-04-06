using System.Collections.Generic;
using UnityEngine;

public class AbilityJoystickManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<AbilityJoystick> joysticks;

    private void Awake()
    {
        var abilityManager = GetComponent<PlayerAbilityManager>();
        abilityManager.OnAbilitiesInit += OnAbilitesInit;
        abilityManager.OnCurrentAbilityChanged += OnCurrentAbilityChanged;
    }

    private void OnAbilitesInit(List<PlayerAbility> abilities)
    {
        joysticks.ForEach((joystick) =>
        {
            abilities.ForEach((ability) =>
            {
                ability.OnCooldownStart += (cd) =>
                {
                    //Debug.Log($"{joystick} is set to {joystick.Ability}");

                    if (joystick.Ability == ability)
                    {
                        joystick.StartCooldownDisplayUpdate(cd);
                    }
                };

                ability.OnCooldownEnd += (cd) =>
                {
                    if (joystick.Ability == ability)
                    {
                        joystick.Input.ToggleInteractablity();
                    }
                };
            });
        });
    }

    private void OnCurrentAbilityChanged(AbilitySwitchEventArgs args)
    {
        var joystick = joysticks[args.index];
        joystick.Ability = args.currentAbility;

        //Debug.Log($"Set {joystick} to {joystick.Ability}.");
    }
}
