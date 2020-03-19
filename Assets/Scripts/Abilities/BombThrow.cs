using System.Collections.Generic;
using UnityEngine;

public class BombThrow : PlayerAbility
{
    protected const float TRAP_COUNT = 3.0f;

    [Header("References")]
    [SerializeField] protected Transform shootPosition;

    [Header("Options")]
    [SerializeField] private int physicsLayer = 10;

    protected List<Projectile> bombPool = new List<Projectile>();
    private int poolIndex;

    private Projectile CurrBomb => bombPool[poolIndex];

    protected override void Awake()
    {
        base.Awake();
        InitBombPool();
    }

    private void InitBombPool()
    {
        for(int i = 0; i < TRAP_COUNT; i++)
        {
            var bomb = InstantiateProjObj();
            bombPool.Add(bomb);
        }
    }

    private Projectile InstantiateProjObj()
    {
        var prefab = Resources.Load<Projectile>("Projectiles/" + GetType().ToString());
        var proj = Instantiate(prefab);
        proj.gameObject.layer = physicsLayer;
        proj.gameObject.SetActive(false);
        return proj;
    }

    public override void OnAbilityRequestStart()
    {
        base.OnAbilityRequestStart();
        poolIndex = (poolIndex + 1) % bombPool.Count;
        CurrBomb.gameObject.SetActive(true);
    }

    public override void OnAbilityRequest(Vector3 direction)
    {
        var clampedDirection = Vector3.ClampMagnitude(direction, Range);
        CurrBomb.transform.position = shootPosition.position;
        base.OnAbilityRequest(clampedDirection);
    }

    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        var velocity = direction.normalized * Speed;
        var bomb = CurrBomb;
        CurrBomb.Shoot(velocity, Range,
            onCollision: (go) =>
            {
                var health = go.GetComponent<PlayerHealth>();
                if (health) health.Value -= Damage;
                bomb.gameObject.SetActive(false);
            });

        base.OnAbilityRequestEnd(direction);
    }


}
