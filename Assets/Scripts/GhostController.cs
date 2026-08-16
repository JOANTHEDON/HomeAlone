using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour {
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _attackRange = 1.5f;     // Distance from door to start attacking
    [SerializeField] private float _attackInterval = 1.0f;  // Attack once per second
    [SerializeField] private float _damagePerAttack = 1.0f; // 1 health point per attack

    private Transform _target;
    private NavMeshAgent _navmeshAgent;
    private DoorHealthManager _doorHealth;
    private float _attackTimer = 0f;

    private void Start() {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;

        GameObject doorObj = GameObject.FindGameObjectWithTag("Door");
        if (doorObj != null) {
            _target = doorObj.transform;
            // Search on object, parent, or children
            _doorHealth = doorObj.GetComponentInParent<DoorHealthManager>();
            if (_doorHealth == null) {
                _doorHealth = doorObj.GetComponentInChildren<DoorHealthManager>();
            }
        } else {
            Debug.LogError("GhostController: No GameObject tagged 'Door' was found in the scene!");
        }
    }

    private void Update() {
        if (_target == null) return;

        Vector2 targetPos = (Vector2)_target.position + _offset;
        float distanceToDoor = Vector2.Distance(transform.position, targetPos);

        // Check if ghost is near the door
        if (distanceToDoor <= _attackRange) {
            // Stop moving when attacking
            if (_navmeshAgent.hasPath) {
                _navmeshAgent.ResetPath();
            }

            // Attack timer (1 attack per second)
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackInterval) {
                _attackTimer = 0f;
                AttackDoor();
            }
        } else {
            // Reset timer and move toward the door
            _attackTimer = 0f;
            _navmeshAgent.SetDestination(targetPos);
        }
    }

    private void AttackDoor() {
        if (_doorHealth != null) {
            _doorHealth.TakeDamage(_damagePerAttack);
            Debug.Log($"Ghost attacked door! Health reduced by {_damagePerAttack}.");
        }
    }
}