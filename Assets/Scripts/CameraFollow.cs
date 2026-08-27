using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float _smoothTime = 0.15f;
    [SerializeField] private float _panSpeed = 0.5f;

    [Header("Zoom Settings")]
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 12f;
    [SerializeField] private float _zoomStep = 1.5f;
    [SerializeField] private float _zoomSmoothTime = 0.1f;

    private Vector3 _velocity = Vector3.zero;
    private bool _isPanEnabled = false;
    private Vector3 _dragOrigin;
    private bool _isDragging = false;
    private Camera _cam;
    private float _targetZoom;
    private float _defaultZoom = 5f;
    private float _zoomVelocity;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;
        if (_cam != null)
        {
            _targetZoom = _cam.orthographicSize;
            _defaultZoom = _cam.orthographicSize;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void EnablePan(bool enable)
    {
        _isPanEnabled = enable;
        if (enable)
        {
            _target = null; // Stop following target when pan is enabled
        }
    }

    public void SetZoomRatio(float ratio)
    {
        // ratio: 0 = Low (Zoom Out / max orthographic size)
        // ratio: 1 = High (Zoom In / min orthographic size)
        ratio = Mathf.Clamp01(ratio);

        if (_cam == null) _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;

        // Directly interpolate between MaxZoom (0) and MinZoom (1)
        float newSize = Mathf.Lerp(_maxZoom, _minZoom, ratio);
        _targetZoom = newSize;

        if (_cam != null && _cam.orthographic)
        {
            _cam.orthographicSize = newSize;
        }
    }

    public void ZoomIn()
    {
        _targetZoom = Mathf.Clamp(_targetZoom - _zoomStep, _minZoom, _maxZoom);
    }

    public void ZoomOut()
    {
        _targetZoom = Mathf.Clamp(_targetZoom + _zoomStep, _minZoom, _maxZoom);
    }

    private void Update()
    {
        if (!_isPanEnabled) return;

        HandlePanInput();
        HandleZoomInput();
    }

    private void HandleZoomInput()
    {
        // Mouse scroll wheel support
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            _targetZoom = Mathf.Clamp(_targetZoom - scrollDelta * 5f, _minZoom, _maxZoom);
        }

        // Pinch-to-zoom support for touch devices
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            _targetZoom = Mathf.Clamp(_targetZoom + deltaMagnitudeDiff * 0.01f, _minZoom, _maxZoom);
        }
    }

    private void SmoothZoom()
    {
        if (_cam == null) _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;

        if (_cam != null && _cam.orthographic)
        {
            _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _targetZoom, ref _zoomVelocity, _zoomSmoothTime);
        }
    }

    private void HandlePanInput()
    {
        // Ignore input if pointer is over UI elements (e.g. turret purchase buttons)
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            _isDragging = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isDragging = true;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = _dragOrigin - currentPos;

            transform.position += new Vector3(difference.x, difference.y, 0f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void LateUpdate()
    {
        SmoothZoom();

        if (!_isPanEnabled && _target != null)
        {
            Vector3 desiredPosition = _target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothTime);
        }
    }
}
