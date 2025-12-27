using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    public PlayerInput playerInput;
    public string lookActionName = "Player/Look"; // Vector2; (Mouse/delta)

    public Transform cameraPitch;

    public float sensitivity = 0.08f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;
    public bool lockCursor = true;

    // debug *bzzz*
    public bool logYawPitch = false;

    private InputAction lookAction;
    private float pitch;

    void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponentInParent<PlayerInput>();

        if (cameraPitch == null)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam) cameraPitch = cam.transform;
        }

        ResolveActions();
    }

    void OnEnable()
    {
        if (lockCursor) SetCursorLock(true);
        ResolveActions();
        lookAction?.Enable();
    }

    void OnDisable()
    {
        lookAction?.Disable();
    }

    void Update()
    {
        Vector2 look = lookAction != null ? lookAction.ReadValue<Vector2>() : Vector2.zero;
        // mouse delta fallback (prevents binding of issac issues)
        if (Mouse.current != null)
        {
            Vector2 raw = Mouse.current.delta.ReadValue();
            if (Mathf.Abs(look.x) < 0.0001f) look.x = raw.x;
            if (Mathf.Abs(look.y) < 0.0001f) look.y = raw.y;
        }

        float mx = look.x * sensitivity;
        float my = look.y * sensitivity;

        // yaw (long)
        transform.Rotate(0f, mx, 0f, Space.Self);

        // pitch (long but sideways)
        if (cameraPitch != null)
        {
            pitch -= my;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
            cameraPitch.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        if (logYawPitch)
            UnityEngine.Debug.Log($"Yaw: {transform.eulerAngles.y:0.0} Pitch: {pitch:0.0}");
    }

    void ResolveActions()
    {
        if (playerInput == null) return;

        var map = playerInput.currentActionMap;
        lookAction = FindActionSmart(map, lookActionName) ?? playerInput.actions?.FindAction(lookActionName, false);
    }

    InputAction FindActionSmart(InputActionMap map, string nameOrPath)
    {
        if (map == null || string.IsNullOrWhiteSpace(nameOrPath)) return null;

        if (nameOrPath.Contains("/"))
        {
            string leaf = nameOrPath[(nameOrPath.LastIndexOf('/') + 1)..];
            var a = map.FindAction(leaf, false);
            if (a != null) return a;
        }

        return map.FindAction(nameOrPath, false);
    }

    public void SetCursorLock(bool shouldLock)
    {
        lockCursor = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
