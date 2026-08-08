using UnityEngine;

public class CradleController : MonoBehaviour {
    [Header("Cradle Visuals")]
    [SerializeField] private SpriteRenderer _cradleSpriteRenderer;
    [SerializeField] private Sprite _closedCradleSprite;

    private bool _hasClosed = false;
    public bool HasClosed => _hasClosed;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasClosed) return;

        // Check if object entering trigger is the Player
        if (collision.CompareTag("Player")) {
            // // 1. Lock player movement
            // PlayerController player = collision.GetComponent<PlayerController>();
            // if (player != null) {
            //     player.isMovementDisabled = true;
            // }

            // 2. Change cradle to closed state (Method A: Sprite Swap)
            if (_cradleSpriteRenderer != null && _closedCradleSprite != null) {
                _cradleSpriteRenderer.sprite = _closedCradleSprite;
            }



            _hasClosed = true;
            Debug.Log("Player entered cradle! Movement locked and cradle closed.");
        }
    }
}