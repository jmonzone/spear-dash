using System;
using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    public const float MAX_HEALTH = 20.0f;

    public Action<float> OnHealthChanged;
    public Action OnHealthZero;

    private float value = MAX_HEALTH;
    public float Value
    {
        get => value;
        private set
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

    public void Damage(float value)
    {
        photonView.RPC(nameof(DamageRPC), RpcTarget.All, value);
    }

    [PunRPC]
    public void DamageRPC(float damage)
    {
        Value -= damage;
    }
}
