using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    public event Action<GameObject> OnCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        col.enabled = false;
    }

    public void Shoot(Vector3 velocity, float range, Action<GameObject> onCollision = null, Action onTargetPositionReached = null)
    {
        rb.velocity = velocity;
        OnCollision = onCollision;
        col.enabled = true;

        StartCoroutine(RangeUpdate(range, onTargetPositionReached));
    }

    private IEnumerator RangeUpdate(float range, Action onComplete = null)
    {
        var start = transform.position;
        yield return new WaitUntil(() => (transform.position - start).magnitude > range);
        rb.velocity = Vector3.zero;
        onComplete?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollision?.Invoke(collision.gameObject);
        StopAllCoroutines();
    }
}
