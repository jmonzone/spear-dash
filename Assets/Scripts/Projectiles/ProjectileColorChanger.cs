using Photon.Pun;
using UnityEngine;

public class ProjectileColorChanger : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDisable()
    {
        spriteRenderer.color = gameObject.layer == 10 ? Color.white : Color.green;
    }
}
