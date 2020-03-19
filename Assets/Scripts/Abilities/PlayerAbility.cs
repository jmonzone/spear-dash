using System;
using System.Collections;
using UnityEngine;

public struct AbilityEventArgs
{
    public Vector3 startPosition;
    public Vector3 direction;
    public float range;
};

public abstract class PlayerAbility : MonoBehaviour
{
    private ScriptableAbility data;
    public Sprite AbilityIcon => data.AbilityIcon;
    public TargetingType TargetType => data.TargetType;
    public AbilityType AbilityType => data.AbilityType;
    private float cooldown;
    protected float Range => data.Range;
    protected float Damage => data.Damage;
    protected float Speed => data.Speed;

    private AudioSource audioSource;

    public bool OnCooldown { get; private set; }

    public event Action<float> OnCooldownStart;
    public event Action<float> OnCooldownEnd;

    public event Action OnAbilityStart;
    public event Action<AbilityEventArgs> OnAbilityAim;
    public event Action<Vector3> OnAbilityEnd;

    protected virtual void Awake()
    {
        data = Resources.Load<ScriptableAbility>("Abilities/" + GetType().ToString());
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        cooldown = GameManager.Instance.TestMode ? 0.0f : data.Cooldown;
    }

    public void Activate()
    {
        StartCoroutine(CooldownUpdate());
    }

    protected IEnumerator CooldownUpdate()
    {
        OnCooldown = true;
        OnCooldownStart?.Invoke(cooldown);
        yield return new WaitForSeconds(cooldown);
        OnCooldown = false;
        OnCooldownEnd?.Invoke(cooldown);
    }

    public virtual void OnAbilityRequestStart()
    {
        OnAbilityStart?.Invoke();
    }

    public virtual void OnAbilityRequest(Vector3 direction)
    {
        var args = new AbilityEventArgs()
        {
            range = Range,
            startPosition = transform.position,
            direction = direction,
        };

        OnAbilityAim?.Invoke(args);
    }

    public virtual void OnAbilityRequestEnd(Vector3 direction)
    {
        OnAbilityEnd?.Invoke(direction);
        audioSource.clip = data.AudioClip;
        audioSource.Play(0);
    }

}
