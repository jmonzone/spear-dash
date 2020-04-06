using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RockPile : PlayerAbility 
{
    private readonly List<Rock> rockPool = new List<Rock>();
    private int poolIndex;
    private Rock CurrRock => rockPool[poolIndex];

    public override void Init(ScriptableAbility ability, Player player)
    {
        base.Init(ability, player);
        InitRockPool();
    }

    private void InitRockPool()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            var proj = InstantiateProjObj();
            rockPool.Add(proj);
        }
    }

    private Rock InstantiateProjObj()
    {
        var proj = PhotonNetwork.Instantiate("Rock", Vector3.zero, Quaternion.identity, 0).GetComponent<Rock>();
        return proj;
    }

    public override void OnAbilityRequestStart()
    {
        base.OnAbilityRequestStart();
        poolIndex = (poolIndex + 1) % rockPool.Count;
    }

    public override void OnAbilityRequest(Vector3 direction)
    {
        var clampedDirection = Vector3.ClampMagnitude(direction, Range);
        base.OnAbilityRequest(clampedDirection);
    }

    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        base.OnAbilityRequestEnd(direction);
        CurrRock.Spawn(player.Position + direction);
    }
}
