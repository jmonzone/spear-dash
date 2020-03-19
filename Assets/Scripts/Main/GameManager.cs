using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Options")]
    [SerializeField] private bool testMode;
    public bool TestMode => testMode;

    [SerializeField] private float joystickSensitivity = 0.5f;
    public float JoystickSensitivity => 10 / joystickSensitivity;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
