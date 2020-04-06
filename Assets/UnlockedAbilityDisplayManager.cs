using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedAbilityDisplayManager : MonoBehaviour
{
    private readonly List<UnlockedAbilityDisplay> abilityDisplays = new List<UnlockedAbilityDisplay>();

    public static event Action<ScriptableAbility> OnAbilitySelected;

    private void Awake()
    {
        GetComponentsInChildren(abilityDisplays);
        abilityDisplays.ForEach((display) => display.gameObject.SetActive(false));
    }

    private void Start()
    {
        InitUnlockedAbilities();
    }

    public void InitUnlockedAbilities()
    {
        var i = 0;
        GameManager.Instance.UnlockedAbilities.ForEach((ability) =>
        {
            var display = abilityDisplays[i];

            display.Init(ability.AbilityIcon, () =>
            {
                OnAbilitySelected?.Invoke(ability);
            });

            display.gameObject.SetActive(true);

            i++;
        });
    }
}
