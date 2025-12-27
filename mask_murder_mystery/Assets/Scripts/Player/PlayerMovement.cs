using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    public string moveActionName = "Player/Move";     // vector2
    public string sprintActionName = "Player/Sprint"; // button

    public float walkSpeed = 4.5f;
    public float sprintSpeed = 7.5f;
    public float acceleration = 14f;
    public float deceleration = 18f;

    public float gravity = -20f;
    public float groundedSnap = -2f;

    public Transform facingRoot;

    private CharacterController controller;
    private InputAction moveAction;
    private InputAction sprintAction;

    private Vector2 moveInput;
    private float verticalVelocity;
    private Vector3 currentHorizontalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (facingRoot == null) facingRoot = transform;

        ResolveActions();
    }

    void OnEnable()
    {
        ResolveActions();
        moveAction?.Enable();
        sprintAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        sprintAction?.Disable();
    }

    void Update()
    {
        moveInput = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;

        bool grounded = controller.isGrounded;
        if (grounded && verticalVelocity < 0f) verticalVelocity = groundedSnap;
        if (!grounded) verticalVelocity += gravity * Time.deltaTime;

        float targetSpeed = (sprintAction != null && sprintAction.IsPressed()) ? sprintSpeed : walkSpeed;

        Vector3 input = new Vector3(moveInput.x, 0f, moveInput.y);
        input = Vector3.ClampMagnitude(input, 1f);

        // move relative to facingroot *toot* *toot*
        Vector3 desiredHorizontal =
            (facingRoot.right * input.x + facingRoot.forward * input.z) * targetSpeed;

        float rate = desiredHorizontal.sqrMagnitude > 0.001f ? acceleration : deceleration;
        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity, desiredHorizontal, rate * Time.deltaTime);

        Vector3 velocity = currentHorizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    void ResolveActions()
    {
        if (playerInput == null) return;

        var map = playerInput.currentActionMap;

        moveAction = FindActionSmart(map, moveActionName) ?? playerInput.actions?.FindAction(moveActionName, false);
        sprintAction = FindActionSmart(map, sprintActionName) ?? playerInput.actions?.FindAction(sprintActionName, false);
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
}
