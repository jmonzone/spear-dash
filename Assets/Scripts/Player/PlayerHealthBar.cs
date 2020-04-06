using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject display;
    [SerializeField] private Transform pivot;
    [SerializeField] private SpriteRenderer fill;

    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponentInParent<PlayerHealth>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= OnHealthChanged;

        display.SetActive(false);
    }

    private void OnHealthChanged(float currentHealth)
    {
        display.SetActive(true);

        var value = currentHealth / PlayerHealth.MAX_HEALTH;

        var newScale = Vector3.one;
        newScale.x = value;
        pivot.transform.localScale = newScale;


        fill.color = Color.Lerp(Color.red, Color.green, value);
    }
}
