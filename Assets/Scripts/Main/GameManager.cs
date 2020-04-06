using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Options")]
    [SerializeField] private bool testMode = false;
    [SerializeField] private float joystickSensitivity = 0.5f;
    [SerializeField] private List<ScriptableAbility> unlockedAbilities;

    public int NumCompPlayers { get; set; }
    public bool TestMode => testMode;
    public float JoystickSensitivity => 10 / joystickSensitivity;
    public List<ScriptableAbility> UnlockedAbilities => unlockedAbilities;

    public List<ScriptableAbility> SelectedAbilities { get; } = new List<ScriptableAbility>();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitSelectedAbilities();

    }

    public void InitSelectedAbilities()
    {
        var jsonAbilities = PlayerPrefs.GetString(PlayerPrefsKeys.PreferredAbilities, JsonUtility.ToJson(new SelectedAbilities()));
        var serializedAbilities = JsonUtility.FromJson<SelectedAbilities>(jsonAbilities);

        foreach (ScriptableAbility ability in UnlockedAbilities)
        {
            if (serializedAbilities.abilities.Contains(ability.name))
                SelectedAbilities.Add(ability);
        }

    }
}

[Serializable]
public class SelectedAbilities
{
    public List<string> abilities = new List<string>();
}



