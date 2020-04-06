using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance { get; private set; }

    [Header("Options")]
    [SerializeField] private Vector2 dimensions;

    private SpriteRenderer sprite;

    public Vector2 Size => new Vector2(sprite.size.x * dimensions.x, sprite.size.y * dimensions.y);
    public Vector2 Dimensions => dimensions;

    private void Awake()
    {
        Instance = this;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
}
