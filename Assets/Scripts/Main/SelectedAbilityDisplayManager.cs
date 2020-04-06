using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectedAbilityDisplayManager : MonoBehaviour
{
    private const int MAX_SELECTED_ABILITIES = 3;

    private readonly List<UnlockedAbilityDisplay> abilityDisplays = new List<UnlockedAbilityDisplay>();

    private void Awake()
    {
        GetComponentsInChildren(abilityDisplays);
        UnlockedAbilityDisplayManager.OnAbilitySelected += OnAbilitySelected;
    }

    private void Start()
    {
        UpdateDisplay(GameManager.Instance.SelectedAbilities);
    }

    private void OnAbilitySelected(ScriptableAbility ability)
    {
        if (GameManager.Instance.SelectedAbilities.Contains(ability)) GameManager.Instance.SelectedAbilities.Remove(ability);
        else if (GameManager.Instance.SelectedAbilities.Count < MAX_SELECTED_ABILITIES) GameManager.Instance.SelectedAbilities.Add(ability);

        SelectedAbilities serializedAbilities = new SelectedAbilities();
        GameManager.Instance.SelectedAbilities.ForEach((x) => serializedAbilities.abilities.Add(x.name));

        var jsonAbilities = JsonUtility.ToJson(serializedAbilities);
        PlayerPrefs.SetString(PlayerPrefsKeys.PreferredAbilities, jsonAbilities);

        UpdateDisplay(GameManager.Instance.SelectedAbilities);
    }

    private void UpdateDisplay(List<ScriptableAbility> abilities)
    {
        abilityDisplays.ForEach((display) => display.gameObject.SetActive(false));

        var i = 0;
        abilities.ForEach((ability) =>
        {
            var display = abilityDisplays[i];

            display.Init(ability.AbilityIcon, () =>
            {
                OnAbilitySelected(ability);
            });

            display.gameObject.SetActive(true);

            i++;
        });
    }
}
