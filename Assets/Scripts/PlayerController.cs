using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    private CoinManager _coinManager;

    private bool isMovementDisabled = false;

    private PlayerLocomotionInput _playerLocomotionInput;
    private Rigidbody2D _rb;
    private SpriteRenderer _playerSprite;
    private Collider2D _playerCollider;

    private void Awake() {

        _rb = GetComponent<Rigidbody2D>();
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerCollider = GetComponent<Collider2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerCollider.enabled = true;
        _playerSprite.enabled = true;
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

    public void DisablePlayerSprite()
{
    if (_playerSprite != null) _playerSprite.enabled = false;
    if (_playerCollider != null) _playerCollider.enabled = false; 
}


}
