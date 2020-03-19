using System;
using System.Collections.Generic;
using UnityEngine;

public struct AbilitySwitchEventArgs
{
    public int index;
    public PlayerAbility previousAbility;
    public PlayerAbility currentAbility;
}

public class PlayerAbilitySwitcher : MonoBehaviour
{
    public List<PlayerAbility> AbilityPool { get; } = new List<PlayerAbility>();
    public List<PlayerAbility> CurrentAbilities = new List<PlayerAbility>();

    public event Action<AbilitySwitchEventArgs> OnCurrentAbilityChanged;

    private void Awake()
    {
        InitAbilities();
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            var random = UnityEngine.Random.Range(0, AbilityPool.Count);

            var args = new AbilitySwitchEventArgs()
            {
                index = i,
                previousAbility = null,
                currentAbility = AbilityPool[random]
            };

            CurrentAbilities.Add(AbilityPool[random]);

            OnCurrentAbilityChanged?.Invoke(args);
            CurrentAbilities[i].Activate();
        }
    }

    private void InitAbilities()
    {
        GetComponentsInChildren(AbilityPool);

        foreach(PlayerAbility ability in AbilityPool)
        {
            ability.OnAbilityEnd += (direction) =>
            {
                if (CurrentAbilities.Contains(ability))
                {
                    var i = CurrentAbilities.FindIndex(x => x == ability);
                    SwitchAbility(i);
                }
            };
        }
    }


    private void SwitchAbility(int i)
    {
        int random;

        do
        {
            random = UnityEngine.Random.Range(0, AbilityPool.Count);

        } while (CurrentAbilities.Contains(AbilityPool[random]));

        var args = new AbilitySwitchEventArgs()
        {
            index = i,
            previousAbility = CurrentAbilities[i],
            currentAbility = AbilityPool[random]
        };

        CurrentAbilities[i] = AbilityPool[random];
        OnCurrentAbilityChanged?.Invoke(args);

        CurrentAbilities[i].Activate();

    }
}
