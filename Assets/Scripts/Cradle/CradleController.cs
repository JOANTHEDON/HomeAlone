using UnityEngine;

public class CradleController : MonoBehaviour {
    [Header("Cradle Visuals")]
    [SerializeField] private SpriteRenderer _cradleSpriteRenderer;
    [SerializeField] private Sprite _closedCradleSprite;
    [SerializeField] private DoorController _doorController;

    private bool _hasClosed = false;
    public bool HasClosed => _hasClosed;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasClosed) return;

        // Check if object entering trigger is the Player
        if (collision.CompareTag("Player")) {
            // 1. Close door if assigned, or find in scene
            if (_doorController == null) {
                _doorController = FindObjectOfType<DoorController>();
            }

            if (_doorController != null) {
                _doorController.CloseDoor();
            }

            // 2. Change cradle to closed state (Method A: Sprite Swap)
            if (_cradleSpriteRenderer != null && _closedCradleSprite != null) {
                _cradleSpriteRenderer.sprite = _closedCradleSprite;
            }

            _hasClosed = true;
            Debug.Log("Player entered cradle! Cradle and door closed.");
        }
    }
}