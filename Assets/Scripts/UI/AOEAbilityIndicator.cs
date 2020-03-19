using UnityEngine;

public class AOEAbilityIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform indicator;

    private void Start()
    {
        foreach (PlayerAbility ability in GetComponents<PlayerAbility>())
        {
            if(ability.TargetType == TargetingType.GROUND)
            {
                ability.OnAbilityStart += () =>
                {
                    indicator.gameObject.SetActive(true);
                };

                ability.OnAbilityAim += (args) =>
                {
                    indicator.position = args.startPosition + args.direction;
                };

                ability.OnAbilityEnd += (direction) =>
                {
                    indicator.gameObject.SetActive(false);
                };
            }
        }
    }

   
}
