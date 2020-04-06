using System;
using System.Collections.Generic;
using UnityEngine;

public struct AbilitySwitchEventArgs
{
    public int index;
    public PlayerAbility currentAbility;
}

public class PlayerAbilityManager : MonoBehaviour
{
    public List<PlayerAbility> AbilityPool { get; } = new List<PlayerAbility>();

    public List<PlayerAbility> CurrentAbilities { get; } = new List<PlayerAbility>();

    public event Action<List<PlayerAbility>> OnAbilitiesInit;
    public event Action<AbilitySwitchEventArgs> OnCurrentAbilityChanged;

    public void Init(Player player)
    {
        OnClientPlayerSpawned(player);
    }

    private void OnClientPlayerSpawned(Player player)
    {
        foreach (ScriptableAbility selectedAbility in GameManager.Instance.SelectedAbilities)
        {
            var type = Type.GetType(selectedAbility.name);
            var ability = gameObject.AddComponent(type) as PlayerAbility;
            ability.Init(selectedAbility, player);
            AbilityPool.Add(ability);
        }

        OnAbilitiesInit?.Invoke(AbilityPool);

        foreach (PlayerAbility ability in AbilityPool)
        {
            ability.OnAbilityEnd += (direction) =>
            {
                if (CurrentAbilities.Contains(ability))
                {
                    var i = CurrentAbilities.FindIndex(x => x == ability);
                    SetAbility(i);
                }
            };
        }

        InitStartingAbilities();
    }

    private void InitStartingAbilities()
    {
        for (int i = 0; i < 2; i++)
        {
            PlayerAbility selectedAbility;
            do
            {
                var random = UnityEngine.Random.Range(0, AbilityPool.Count);
                selectedAbility = AbilityPool[random];

            } while (CurrentAbilities.Contains(selectedAbility));

            var args = new AbilitySwitchEventArgs()
            {
                index = i,
                currentAbility = selectedAbility,
            };

            CurrentAbilities.Add(selectedAbility);
            OnCurrentAbilityChanged?.Invoke(args);
            //Debug.Log($"{selectedAbility} activating.");
            selectedAbility.Activate();
        }
    }

    private void SetAbility(int i)
    {
        PlayerAbility selectedAbility;
        do
        {
            var random = UnityEngine.Random.Range(0, AbilityPool.Count);
            selectedAbility = AbilityPool[random];

        } while (CurrentAbilities.Contains(selectedAbility));

        var args = new AbilitySwitchEventArgs()
        {
            index = i,
            currentAbility = selectedAbility
        };

        CurrentAbilities[i] = selectedAbility;
        OnCurrentAbilityChanged?.Invoke(args);

        selectedAbility.Activate();

    }
}
