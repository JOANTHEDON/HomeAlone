using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    //[SerializeField] private float _ghostSpeed = 5f;
    private Transform _target;
    [SerializeField]private Vector2 _offset;
    private NavMeshAgent _navmeshAgent;

    private bool _shouldMove = false;
    
    public bool ShouldMove => _shouldMove;
    private void Start()
    {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;
        _target = GameObject.FindGameObjectWithTag("Door").transform; 
        
    }

    private void Update()
    {
        if(_target == null ) return;
        // Vector2 currentPos = transform.position;
        Vector2 targetPos = (Vector2)_target.position + _offset;
        // transform.position = Vector2.MoveTowards(currentPos,targetPos, _ghostSpeed * Time.deltaTime);
        _navmeshAgent.SetDestination(targetPos);

    }

    
}
