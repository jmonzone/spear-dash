using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class ProjectileAbility : PlayerAbility
{
    private readonly List<Projectile> projPool = new List<Projectile>();
    private int poolIndex;
    private Projectile CurrProj => projPool[poolIndex];

    public override void Init(ScriptableAbility ability, Player  player)
    {
        base.Init(ability, player);
        InitBombPool();
    }

    private void InitBombPool()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            var proj = InstantiateProjObj(GetType().ToString());
            projPool.Add(proj);
        }
    }

    private Projectile InstantiateProjObj(string projName)
    {
        var proj = PhotonNetwork.Instantiate("Projectiles/" + projName, Vector3.zero, Quaternion.identity, 0).GetComponent<Projectile>();

        var layer = player.IsMine && !player.IsComputer ? 10 : 11;
        proj.Init(layer);
        return proj;
    }

    public override void OnAbilityRequestStart()
    {
        base.OnAbilityRequestStart();
        poolIndex = (poolIndex + 1) % projPool.Count;
        CurrProj.Spawn(player.Position);
    }

    public override void OnAbilityRequest(Vector3 direction)
    {
        var clampedDirection = Vector3.ClampMagnitude(direction, Range);
        CurrProj.transform.position = player.Position;
        if (TargetType == TargetingType.DIRECTION) CurrProj.transform.up = direction;
        base.OnAbilityRequest(clampedDirection);
    }

    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        var velocity = direction.normalized * Speed;
        var cachedProj = CurrProj;
        CurrProj.Shoot(velocity, Range, onCollision: (go) =>
        {
            var health = go.GetComponent<PlayerHealth>();
            if (health) health.Damage(Damage);

            //var player = go.GetComponent<Player>();
            //if (player) player.Knockback(velocity * 1000.0f);

            cachedProj.Display(false);
        });

        base.OnAbilityRequestEnd(direction);
    }

    public override void OnAbilityRequestCancel()
    {
        base.OnAbilityRequestCancel();
        CurrProj.gameObject.SetActive(false);
    }
}
