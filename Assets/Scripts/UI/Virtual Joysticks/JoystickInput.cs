using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInput : MonoBehaviour, IPointerDownHandler
{
    private float sensitivity;

    public event Action OnDirectionSelectStart;
    public event Action<Vector3> OnDirectionIsSelecting;
    public event Action<Vector3> OnDirectionSelectEnd;

    private bool interactable = true;

    private void Start()
    {
        sensitivity = GameManager.Instance.JoystickSensitivity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable) StartCoroutine(InputUpdate());
    }

    private IEnumerator InputUpdate()
    {
        var startPosition = Input.mousePosition;
        Vector3 endPosition, direction = Vector3.zero;

        OnDirectionSelectStart?.Invoke();

        while (Input.GetMouseButton(0))
        {
            yield return new WaitForFixedUpdate();

            endPosition = Input.mousePosition;
            direction = endPosition - startPosition;
            direction /= sensitivity;
            OnDirectionIsSelecting?.Invoke(direction);
        }

        if(direction != Vector3.zero)
            OnDirectionSelectEnd?.Invoke(direction);
    }

    public void ToggleInteractablity(bool interactable = true)
    {
        this.interactable = interactable;
    }

    public void ResetListeners()
    {
        OnDirectionSelectStart = null;
        OnDirectionIsSelecting = null;
        OnDirectionSelectEnd = null;
    }
}
