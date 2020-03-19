using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileColorChanger : MonoBehaviour
{
    private void OnEnable()
    {
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = gameObject.layer == 10 ? Color.white : Color.green;
    }
}
