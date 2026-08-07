using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField]private CoinManager _coinManager;

    private PlayerLocomotionInput _playerLocomotionInput;
    private Rigidbody2D _rb;

    private void Awake() {
        
        _rb = GetComponent<Rigidbody2D>();
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        if(_coinManager == null) return;
    }

    private void FixedUpdate() {
        _rb.linearVelocity = _playerLocomotionInput.MovementInput * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            _coinManager.StartCoinSpawn = true;
            Debug.Log("player colliding with door");
        }
    }
}
