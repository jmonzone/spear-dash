using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int index = 0;
    [SerializeField] private PlayerAbilitySwitcher abilitySwitcher;
    [SerializeField] private Image cooldownMask;
    [SerializeField] private Image abilityIcon;

    private JoystickInput input;

    private void Awake()
    {
        input = GetComponent<JoystickInput>();
        abilitySwitcher.OnCurrentAbilityChanged += OnAbilityChanged;
    }

    private void OnAbilityChanged(AbilitySwitchEventArgs args)
    {
        if (args.index != index) return;

        if(args.previousAbility != null)
        {
            input.OnDirectionSelectStart -= args.previousAbility.OnAbilityRequestStart;
            input.OnDirectionIsSelecting -= args.previousAbility.OnAbilityRequest;
            input.OnDirectionSelectEnd -= args.previousAbility.OnAbilityRequestEnd;

            args.previousAbility.OnCooldownStart -= StartCooldownDisplayUpdate;
            args.previousAbility.OnCooldownEnd -= (x) => input.ToggleInteractablity();
        }

        input.OnDirectionSelectStart += args.currentAbility.OnAbilityRequestStart;
        input.OnDirectionIsSelecting += args.currentAbility.OnAbilityRequest;
        input.OnDirectionSelectEnd += args.currentAbility.OnAbilityRequestEnd;

        input.ToggleInteractablity(false);

        cooldownMask.sprite = abilityIcon.sprite = args.currentAbility.AbilityIcon;

        args.currentAbility.OnCooldownStart += StartCooldownDisplayUpdate;
        args.currentAbility.OnCooldownEnd += (x) => input.ToggleInteractablity();

        //Debug.Log(args.index + " " + args.previousAbility + " " + args.currentAbility);
        //Debug.Log(args.currentAbility.OnCooldownStart);
    }

    private void StartCooldownDisplayUpdate(float cooldown)
    {
        StartCoroutine(CooldownUpdate(cooldown));
    }

    private IEnumerator CooldownUpdate(float cooldown)
    {
        var startTime = Time.time;
        while(Time.time - startTime < cooldown)
        {
            abilityIcon.fillAmount = (Time.time - startTime) / cooldown;
            yield return new WaitForFixedUpdate();
        }
        abilityIcon.fillAmount = 1.0f;
    }
}
