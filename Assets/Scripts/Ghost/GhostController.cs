using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour {
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _attackRange = 1.5f;     // Distance from door to start attacking
    [SerializeField] private float _attackInterval = 1.0f;  // Attack once per second
    [SerializeField] private float _damagePerAttack = 1.0f; // 1 health point per attack
    private Transform _cradleTarget;

    private Transform _target;
    private NavMeshAgent _navmeshAgent;
    private DoorHealthManager _doorHealth;
    private float _attackTimer = 0f;
    private bool _isGameOver = false;

    public bool IsGameOver => _isGameOver;

    private void Start() {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;

        GameObject doorObj = GameObject.FindGameObjectWithTag("Door");
        GameObject cradleObj = GameObject.FindGameObjectWithTag("Cradle");
        _cradleTarget = cradleObj.transform;
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

    private void Update()
    {
        if (_navmeshAgent == null) return;

        if (_doorHealth != null && _doorHealth.IsDoorBroken)
        {
            if (_cradleTarget != null)
            {
                _navmeshAgent.SetDestination(_cradleTarget.position);
            }

            if (_cradleTarget != null && Vector2.Distance(transform.position, _cradleTarget.position) < 0.8f)
            {
                _isGameOver = true;
            }

            return;
        }

        if (_target == null) return;

        Vector2 targetPos = (Vector2)_target.position + _offset;
        float distanceToDoor = Vector2.Distance(transform.position, targetPos);

        if (distanceToDoor <= _attackRange)
        {
            if (_navmeshAgent.hasPath)
                _navmeshAgent.ResetPath();

            _attackTimer += Time.deltaTime;

            if (_attackTimer >= _attackInterval)
            {
                _attackTimer = 0f;
                AttackDoor();
            }
        }
        else
        {
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