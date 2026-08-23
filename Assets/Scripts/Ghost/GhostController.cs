using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour {
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _attackRange = 1.5f;     // Distance from door to start attacking
    [SerializeField] private float _attackInterval = 1.0f;  // Attack once per second
    [SerializeField] private float _damagePerAttack = 1.0f; // 1 health point per attack
    [Header("Heal & Retreat Settings")]
    [SerializeField] private float _healThreshold = 30f;        // Retreat when health falls between 30 and 60 (or <= 60)
    [SerializeField] private float _healRatePercentPerSecond = 0.10f; // 10% per second
    [SerializeField] private GameObject _healLocation;         // Location to retreat to for healing (GameObject or Transform)

    private Transform _cradleTarget;
    private Transform _target;
    private NavMeshAgent _navmeshAgent;
    private DoorHealthManager _doorHealth;
    private GhostHealth _ghostHealth;
    private float _attackTimer = 0f;
    private bool _isGameOver = false;
    private bool _isHealingState = false;
    private Vector3 _initialSpawnPos;

    public bool IsGameOver => _isGameOver;

    private void Start() {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;
        _ghostHealth = GetComponent<GhostHealth>();
        _initialSpawnPos = transform.position;

        // If no explicit heal location assigned, search for object with tag "HealPoint" or fallback to spawn position
        if (_healLocation == null) {
            _healLocation = GameObject.FindGameObjectWithTag("HealPoint");
        }

        GameObject doorObj = GameObject.FindGameObjectWithTag("Door");
        GameObject cradleObj = GameObject.FindGameObjectWithTag("Cradle");
        if (cradleObj != null) _cradleTarget = cradleObj.transform;
        
        if (doorObj != null) {
            _target = doorObj.transform;
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

        // --- RETREAT & HEALING LOGIC ---
        if (_ghostHealth != null)
        {
            // Trigger retreat state when health is at or below threshold
            if (!_isHealingState && _ghostHealth.CurrentHealth <= _healThreshold)
            {
                _isHealingState = true;
                Debug.Log($"Ghost health is low ({_ghostHealth.CurrentHealth}/{_ghostHealth.MaxHealth}). Retreating to heal!");
            }

            if (_isHealingState)
            {
                Vector3 healDestination = _healLocation != null ? _healLocation.transform.position : _initialSpawnPos;
                _navmeshAgent.SetDestination(healDestination);

                // Check if reached heal destination
                float distToHealSpot = Vector2.Distance(transform.position, healDestination);
                if (distToHealSpot <= 1.2f)
                {
                    // Heal 10% of Max Health per second
                    float healAmount = _ghostHealth.MaxHealth * _healRatePercentPerSecond * Time.deltaTime;
                    _ghostHealth.Heal(healAmount);

                    // Once fully healed, resume attack
                    if (_ghostHealth.CurrentHealth >= _ghostHealth.MaxHealth)
                    {
                        _isHealingState = false;
                        Debug.Log("Ghost fully healed! Resuming attack.");
                    }
                }

                return; // Do not attack while in retreat/healing mode
            }
        }

        // --- NORMAL ATTACK LOGIC ---
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