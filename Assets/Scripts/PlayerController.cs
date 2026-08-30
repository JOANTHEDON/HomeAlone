using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    private CoinManager _coinManager;

    private bool isMovementDisabled = false;

    private PlayerLocomotionInput _playerLocomotionInput;
    private Rigidbody2D _rb;

    private void Awake() {

        _rb = GetComponent<Rigidbody2D>();
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _coinManager = FindAnyObjectByType<CoinManager>();
        if (_coinManager == null) return;

    }

    private void FixedUpdate() {
        if (isMovementDisabled) {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        _rb.linearVelocity = _playerLocomotionInput.MovementInput * moveSpeed;
    }
}
