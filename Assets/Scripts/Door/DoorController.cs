using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour {
    [SerializeField] private SpriteRenderer DoorspriteRenderer;
    [SerializeField] private Sprite DoorClosed;
    [SerializeField]private Sprite DoorOpen;
    [SerializeField]private DoorHealthManager _doorHealthmanager;
    private NavMeshObstacle _obstacle;

    private bool _doorClosed= false;

    public bool IsDoorClosed => _doorClosed;

    private void Awake()
    {
        _doorClosed = false;
        _doorHealthmanager=GetComponent<DoorHealthManager>();
        _obstacle = GetComponent<NavMeshObstacle>();
        _obstacle.enabled = true;

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            DoorspriteRenderer.sprite = DoorClosed;
            _doorClosed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _doorHealthmanager.IsDoorBroken)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (DoorspriteRenderer != null)
            DoorspriteRenderer.sprite = DoorOpen;

        if (_obstacle != null)
            _obstacle.enabled = false;

        _doorClosed = false;
    }

}
