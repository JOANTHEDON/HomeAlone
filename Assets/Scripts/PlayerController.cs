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

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Door")) return;

        if (_coinManager == null) {
            Debug.LogWarning("CoinManager not assigned on PlayerController; cannot start coin spawn.");
            return;
        }

        _coinManager.StartCoinSpawn = true;
        Debug.Log("player colliding with door");
    }
}
