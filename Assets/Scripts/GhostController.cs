using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float _ghostSpeed = 5f;
    private Transform _target;
    [SerializeField]private Vector2 _offset;

    private bool _shouldMove = false;
    
    public bool ShouldMove => _shouldMove;
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Door").transform; 
    }

    private void Update()
    {
        if(_target == null ) return;
        Vector2 currentPos = transform.position;
        Vector2 targetPos = (Vector2)_target.position + _offset;
        transform.position = Vector2.MoveTowards(currentPos,targetPos, _ghostSpeed * Time.deltaTime);

    }
}
