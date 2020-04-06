using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler
{
    private float sensitivity;

    public event Action OnDirectionSelectStart;
    public event Action<Vector3> OnDirectionIsSelecting;
    public event Action<Vector3> OnDirectionSelectEnd;
    public event Action OnDirectionCanceled;

    public bool interactable = true;

    private DeviceType device;

    private void Start()
    {
        sensitivity = GameManager.Instance.JoystickSensitivity;
        device = SystemInfo.deviceType;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;

        if (device == DeviceType.Desktop)
            StartCoroutine(DesktopInputUpdate());
        else if(device == DeviceType.Handheld)
            StartCoroutine(HandheldInputUpdate());
    }

    private IEnumerator DesktopInputUpdate()
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

        if (direction != Vector3.zero)
            OnDirectionSelectEnd?.Invoke(direction);
        else
            OnDirectionCanceled?.Invoke();
    }

    private Touch GetTouch(int id)
    {
        Touch retval = Input.GetTouch(0);
        for(int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == id) retval = Input.GetTouch(i);
        }
        return retval;
    }

    private IEnumerator HandheldInputUpdate()
    {
        var i = Input.GetTouch(Input.touchCount - 1).fingerId;

        Vector3 startPosition = GetTouch(i).position;
        Vector3 endPosition, direction = Vector3.zero;

        Debug.Log($"Touch start");
        OnDirectionSelectStart?.Invoke();

        yield return new WaitUntil(() => GetTouch(i).phase == TouchPhase.Moved || GetTouch(i).phase == TouchPhase.Stationary);

        while (GetTouch(i).phase == TouchPhase.Moved || GetTouch(i).phase == TouchPhase.Stationary)
        {
            yield return new WaitForFixedUpdate();

            endPosition = GetTouch(i).position;
            direction = endPosition - startPosition;
            direction /= sensitivity;

            Debug.Log($"Touch down");
            OnDirectionIsSelecting?.Invoke(direction);
        }

        if (direction != Vector3.zero)
        {
            Debug.Log($"Touch end");
            OnDirectionSelectEnd?.Invoke(direction);

        }
        else
        {
            Debug.Log($"Touch canceled");
            OnDirectionCanceled?.Invoke();
        }
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
