using UnityEngine;

public enum AbilityType
{
    DASH,
    DAMAGE,
    TRAP,
}

public enum TargetingType
{
    DIRECTION,
    GROUND,
}

[CreateAssetMenu(fileName = "Scriptable Ability", menuName = "ScriptableObjects/Scriptable Ability")]
public class ScriptableAbility : ScriptableObject
{
    [SerializeField] private Sprite abilityIcon;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private TargetingType targetType;
    [SerializeField] private float cooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private int projectileCount;

    public Sprite AbilityIcon => abilityIcon;
    public AudioClip AudioClip => audioClip;
    public AbilityType AbilityType => abilityType;
    public TargetingType TargetType => targetType;
    public float Cooldown => cooldown;
    public float Range => range;
    public float Damage => damage;
    public float Speed => speed;
    public int ProjectileCount => projectileCount;
}