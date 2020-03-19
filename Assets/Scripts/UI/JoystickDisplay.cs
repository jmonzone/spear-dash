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
        var input = GetComponent<JoystickInput>();
        input.OnDirectionSelectStart += () =>
        {
            stick.gameObject.SetActive(true);
        };

        input.OnDirectionIsSelecting += (direction) =>
        {
            var distance = Input.mousePosition - background.transform.position;
            if (distance.magnitude > stickRadius) distance = distance.normalized * stickRadius;
            stick.transform.position = background.transform.position + distance;

            arrow.gameObject.SetActive(true);

            arrow.up = -direction;
            arrow.position = background.transform.position + direction.normalized * 200.0f;
        };

        input.OnDirectionSelectEnd += (direction) =>
        {
            if (hideJoystick) stick.gameObject.SetActive(false);
            stick.transform.position = background.position;
            arrow.gameObject.SetActive(false);
        };

        arrow.gameObject.SetActive(false);
    }
}
