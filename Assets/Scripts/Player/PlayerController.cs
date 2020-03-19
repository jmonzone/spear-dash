using System;
using System.Collections;
using UnityEngine;

public interface IController
{
    event Action OnWalkRequestStart;
    event Action OnWalkRequestEnd;
    event Action<Vector3> OnWalkRequest;
}

public class PlayerController : MonoBehaviour, IController
{
    [Header("References")]
    [SerializeField] private JoystickInput walkJoystick;

    public event Action OnWalkRequestStart;
    public event Action OnWalkRequestEnd;
    public event Action<Vector3> OnWalkRequest;

    private void Awake()
    {
        walkJoystick.OnDirectionSelectStart += () => OnWalkRequestStart();
        walkJoystick.OnDirectionIsSelecting += (direction) => OnWalkRequest(direction);
        walkJoystick.OnDirectionSelectEnd += (direction) => OnWalkRequestEnd();

        StartCoroutine(MovementUpdate());
    }

    private IEnumerator MovementUpdate()
    {
        while (true)
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var direction = new Vector3(x, y).normalized;

            if (direction != Vector3.zero)
            {
                OnWalkRequestStart();

                while(direction != Vector3.zero)
                {
                    x = Input.GetAxis("Horizontal");
                    y = Input.GetAxis("Vertical");
                    direction = new Vector3(x, y).normalized;
                    OnWalkRequest(direction);
                    yield return new WaitForFixedUpdate();
                }

                OnWalkRequestEnd();
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
