using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    private PlayerLocomotionInput playerLocomotionInput;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void FixedUpdate() {
        rb.linearVelocity = playerLocomotionInput.MovementInput * moveSpeed;
    }
}
