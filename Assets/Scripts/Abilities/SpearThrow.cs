using UnityEngine;

public class ProjectileAbility : PlayerAbility
{
    [Header("References")]
    [SerializeField] protected Transform shootPosition;

    [Header("Options")]
    [SerializeField] private int physicsLayer = 10;

    protected Projectile projectile;

    protected override void Awake()
    {
        base.Awake();
        projectile = InstantiateProjObj();
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
        projectile.gameObject.SetActive(true);
    }

    public override void OnAbilityRequest(Vector3 direction)
    {
        base.OnAbilityRequest(direction);
        projectile.transform.position = shootPosition.position;
        projectile.transform.up = direction;
    }  
}

public class SpearThrow : ProjectileAbility
{
    public override void OnAbilityRequestEnd(Vector3 direction)
    {
        var velocity = direction.normalized * Speed;
        projectile.Shoot(velocity, Range, onCollision: (go) =>
        {
            var health = go.GetComponent<PlayerHealth>();
            if (health) health.Value -= Damage;
            projectile.gameObject.SetActive(false);
        });

        base.OnAbilityRequestEnd(direction);
    }
}
