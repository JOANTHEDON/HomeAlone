using UnityEngine;
using UnityEngine.UI;

public class CradleController : MonoBehaviour {
    [Header("Cradle Visuals")]
    [SerializeField] private SpriteRenderer _cradleSpriteRenderer;
    [SerializeField] private Sprite _closedCradleSprite;
    [SerializeField] private DoorController _doorController;

    [Header("UI Settings")]
    [SerializeField] private GameObject _sleepButton;

    private bool _hasClosed = false;
    public bool HasClosed => _hasClosed;

    private void Start() {
        if (_sleepButton != null) {
            _sleepButton.SetActive(false);

            Button btn = _sleepButton.GetComponent<Button>();
            if (btn == null) {
                btn = _sleepButton.GetComponentInChildren<Button>();
            }

            if (btn != null) {
                btn.onClick.AddListener(OnSleepButtonClicked);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_hasClosed) return;

        if (collision.CompareTag("Player")) {
            if (_sleepButton != null) {
                _sleepButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (_hasClosed) return;

        if (collision.CompareTag("Player")) {
            if (_sleepButton != null) {
                _sleepButton.SetActive(false);
            }
        }
    }

    public void OnSleepButtonClicked() {
        if (_hasClosed) return;

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

        // 3. Hide the sleep button
        if (_sleepButton != null) {
            _sleepButton.SetActive(false);
        }

        // 4. Start coin spawning now that player is sleeping
        CoinManager coinManager = FindAnyObjectByType<CoinManager>();
        if (coinManager != null) {
            coinManager.StartCoinSpawn = true;
        }

        _hasClosed = true;
        Debug.Log("Sleep button clicked! Cradle and door closed, coin system started.");
    }
}