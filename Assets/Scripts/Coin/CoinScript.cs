using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private Vector2 _startPoint, _endPoint;
    private float _transitionTime;
    private float _elapsedTime  = 0f;
    private bool _isMoving = false;

    
    void Update()
    {
        if(_isMoving)
            CoinMovement();
    }

    public void Initialize(Vector2 startPosition, Vector2 endPosition, float timeDuration)
    {
        _startPoint = startPosition;
        _endPoint = endPosition;
        _transitionTime = timeDuration;
        _elapsedTime = 0f;
        _isMoving = true;
        gameObject.SetActive(true);
        transform.position = _startPoint;
    }

    public void CoinMovement()
    {
        _elapsedTime += Time.deltaTime;
        float t = _elapsedTime/_transitionTime;
        transform.position = Vector2.Lerp(_startPoint,_endPoint, t);
        if(t>= 1f)
        {
            _isMoving = false;
            gameObject.SetActive(false);
        }
    }
}
