using UnityEngine;

public class DoorController : MonoBehaviour {
    [SerializeField] private SpriteRenderer DoorspriteRenderer;
    [SerializeField] private Sprite DoorClosed;
    private bool _doorClosed= false;

    public bool IsDoorClosed => _doorClosed;

    private void Awake()
    {
        _doorClosed = false;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            DoorspriteRenderer.sprite = DoorClosed;
            _doorClosed = true;
        }
    }

}
