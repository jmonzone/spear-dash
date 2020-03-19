using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public const float MAX_HEALTH = 20.0f;

    public Action<float> OnHealthChanged;
    public Action OnHealthZero;

    private float value = MAX_HEALTH;
    public float Value
    {
        get => value;
        set
        {
            this.value = Mathf.Clamp(value, 0, MAX_HEALTH);

            if(value < MAX_HEALTH)
            {
                audioSource.clip = audioClip;
                audioSource.Play(0);
            }

            OnHealthChanged?.Invoke(this.value);

            if (this.value == 0)
            {
                OnHealthZero?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    private AudioSource audioSource;
    public AudioClip audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
