using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions {
    [SerializeField] private Joystick joystick;

    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementInput { get; private set; }

    private Vector2 keyboardInput;

    private void Start() {
        if (joystick == null) {
            joystick = FindAnyObjectByType<Joystick>();
        }
    }

    private void OnEnable() {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable() {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    private void Update() {
        Vector2 input = Vector2.zero;

        // 1. Read Joystick input if active
        if (joystick != null) {
            input.x = joystick.Horizontal;
            input.y = joystick.Vertical;
        }

        // 2. Fallback to Input System keyboard input if joystick is idle
        if (input.sqrMagnitude < 0.001f) {
            input = keyboardInput;
        }

        MovementInput = input;
    }

    public void OnMovement(InputAction.CallbackContext context) {
        keyboardInput = context.ReadValue<Vector2>();
    }
}
