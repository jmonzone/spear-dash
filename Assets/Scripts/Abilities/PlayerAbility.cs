using System;
using System.Collections;
using Photon.Pun;
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
    public float Cooldown => GameManager.Instance.TestMode ? 0.0f : data.Cooldown;
    public float Range => data.Range;
    protected float Damage => data.Damage;
    protected float Speed => data.Speed;
    protected float ProjectileCount => data.ProjectileCount;

    private AudioSource audioSource;
    protected ParticleSystem particle;
    protected Player  player;

    public event Action<float> OnCooldownStart;
    public event Action<float> OnCooldownEnd;

    public event Action<Vector3> OnAbilityStart;
    public event Action<AbilityEventArgs> OnAbilityAim;
    public event Action<Vector3> OnAbilityEnd;
    public event Action OnAbilityCancel;

    public virtual void Init(ScriptableAbility data, Player player)
    {
        this.data = data;
        this.player = player;
        audioSource = player.GetComponent<AudioSource>();
        particle = player.GetComponentInChildren<ParticleSystem>();
    }

    public void Activate()
    {
        StartCoroutine(CooldownUpdate());
    }

    protected IEnumerator CooldownUpdate()
    {
        OnCooldownStart?.Invoke(Cooldown);
        //Debug.Log($"{name} {GetType()} cooldown has started for {Cooldown} seconds.");
        yield return new WaitForSeconds(Cooldown);
        //Debug.Log($"{name} {GetType()} cooldown has ended.");
        OnCooldownEnd?.Invoke(Cooldown);
    }

    public virtual void OnAbilityRequestStart()
    {
        OnAbilityStart?.Invoke(player.transform.position);
    }

    public virtual void OnAbilityRequest(Vector3 direction)
    {
        var args = new AbilityEventArgs()
        {
            range = Range,
            startPosition = player.transform.position,
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

    public virtual void OnAbilityRequestCancel()
    {
        OnAbilityCancel?.Invoke();
    }

}
