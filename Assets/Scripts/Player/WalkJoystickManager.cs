using System.Collections;
using UnityEngine;

public class WalkJoystickManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick input;

    private void Awake()
    {
        var movement = GetComponent<PlayerMovementManager>();

        input.OnDirectionSelectStart += movement.OnWalkRequestStart;
        input.OnDirectionIsSelecting += movement.OnWalkRequest;
        input.OnDirectionSelectEnd += movement.OnWalkRequestEnd;
        input.OnDirectionCanceled += movement.OnWalkRequestCancel;

        if (SystemInfo.deviceType == DeviceType.Desktop)
            StartCoroutine(MovementUpdate(movement));
    }

    private IEnumerator MovementUpdate(PlayerMovementManager movement)
    {
        while (true)
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var direction = new Vector3(x, y).normalized;

            if (direction != Vector3.zero)
            {
                movement.OnWalkRequestStart();

                while (direction != Vector3.zero)
                {
                    x = Input.GetAxis("Horizontal");
                    y = Input.GetAxis("Vertical");
                    direction = new Vector3(x, y).normalized;
                    movement.OnWalkRequest(direction);
                    yield return new WaitForFixedUpdate();
                }

                movement.OnWalkRequestEnd(direction);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
