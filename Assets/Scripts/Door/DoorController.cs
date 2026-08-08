using UnityEngine;

public class DoorController : MonoBehaviour {
    [SerializeField] private SpriteRenderer DoorspriteRenderer;
    [SerializeField] private Sprite DoorClosed;

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            DoorspriteRenderer.sprite = DoorClosed;
        }
    }

}
