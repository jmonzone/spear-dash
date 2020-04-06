using UnityEngine;

public class JoystickDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform background;
    [SerializeField] private Transform stick;
    [SerializeField] private Transform arrow;
    [SerializeField] private float stickRadius = 100.0f;

    [Header("Options")]
    [SerializeField] private bool hideJoystick = false;

    private void Awake()
    {
        var input = GetComponent<Joystick>();
        input.OnDirectionSelectStart += () =>
        {
            stick.gameObject.SetActive(true);
        };

        input.OnDirectionIsSelecting += (direction) =>
        {
            direction *= GameManager.Instance.JoystickSensitivity;
            if (direction.magnitude > stickRadius) direction = direction.normalized * stickRadius;
            stick.transform.position = background.transform.position + direction;

            arrow.gameObject.SetActive(true);

            arrow.up = -direction;
            arrow.position = background.transform.position + direction.normalized * 200.0f;
        };

        input.OnDirectionSelectEnd += (direction) => ResetDisplay();
        input.OnDirectionCanceled += () => ResetDisplay();

        arrow.gameObject.SetActive(false);
    }

    public void ResetDisplay()
    {
        if (hideJoystick) stick.gameObject.SetActive(false);
        stick.transform.position = background.position;
        arrow.gameObject.SetActive(false);
    }
}
