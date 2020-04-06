using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityJoystick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private Image abilityImage;
    [SerializeField] private Image cooldownImage;

    public Joystick Input => joystick;
    public Image AbilityImage => abilityImage;
    public Image CooldownImage => cooldownImage;

    private PlayerAbility ability;
    public PlayerAbility Ability
    {
        get => ability;
        set
        {
            if (ability)
            {
                Input.OnDirectionSelectStart -= ability.OnAbilityRequestStart;
                Input.OnDirectionIsSelecting -= ability.OnAbilityRequest;
                Input.OnDirectionSelectEnd -= ability.OnAbilityRequestEnd;
                Input.OnDirectionCanceled -= ability.OnAbilityRequestCancel;
            }

            ability = value;

            Input.ToggleInteractablity(false);

            Input.OnDirectionSelectStart += ability.OnAbilityRequestStart;
            Input.OnDirectionIsSelecting += ability.OnAbilityRequest;
            Input.OnDirectionSelectEnd += ability.OnAbilityRequestEnd;
            Input.OnDirectionCanceled += ability.OnAbilityRequestCancel;

            CooldownImage.sprite = AbilityImage.sprite = ability.AbilityIcon;
        }
    }

    public void StartCooldownDisplayUpdate(float cooldown)
    {
        //Debug.Log($"{name} starting cooldown display");
        StartCoroutine(CooldownDisplayUpdate(cooldown));
    }

    private IEnumerator CooldownDisplayUpdate(float cooldown)
    {
        var startTime = Time.time;
        while (Time.time - startTime < cooldown)
        {
            abilityImage.fillAmount = (Time.time - startTime) / cooldown;
            yield return new WaitForFixedUpdate();
        }
        abilityImage.fillAmount = 1.0f;
    }
}
