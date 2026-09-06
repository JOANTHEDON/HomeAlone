using UnityEngine;
using UnityEngine.InputSystem;

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
        HandleZoomInput(); // Allow zooming all the time

        if (!_isPanEnabled) return;

        HandlePanInput();
    }

    private void HandleZoomInput()
    {
        // Mouse scroll wheel support
        if (Mouse.current != null)
        {
            float scrollDelta = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                // New Input System scroll returns larger values (e.g., 120 per tick), scale it down:
                _targetZoom = Mathf.Clamp(_targetZoom - scrollDelta * 0.005f, _minZoom, _maxZoom);
            }
        }

        // Pinch-to-zoom support for touch devices
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch0 = Touchscreen.current.touches[0];
            var touch1 = Touchscreen.current.touches[1];

            if (touch0.press.isPressed && touch1.press.isPressed)
            {
                Vector2 touchZeroPos = touch0.position.ReadValue();
                Vector2 touchOnePos = touch1.position.ReadValue();
                Vector2 touchZeroDelta = touch0.delta.ReadValue();
                Vector2 touchOneDelta = touch1.delta.ReadValue();

                Vector2 touchZeroPrevPos = touchZeroPos - touchZeroDelta;
                Vector2 touchOnePrevPos = touchOnePos - touchOneDelta;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZeroPos - touchOnePos).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // Adjust the multiplier here if pinching is too slow or too fast
                _targetZoom = Mathf.Clamp(_targetZoom + deltaMagnitudeDiff * 0.05f, _minZoom, _maxZoom);
            }
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
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            bool pointerOverUI = false;
            if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].press.isPressed)
            {
                pointerOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue());
            }
            else
            {
                pointerOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            }
            
            if (pointerOverUI)
            {
                _isDragging = false;
                return;
            }
        }

        Vector2 pointerPosition = Vector2.zero;
        bool isPressDown = false;
        bool isPressed = false;
        bool isPressUp = false;

        bool touchActive = Touchscreen.current != null && (Touchscreen.current.primaryTouch.press.isPressed || Touchscreen.current.primaryTouch.press.wasReleasedThisFrame);

        if (touchActive)
        {
            pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isPressDown = Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
            isPressed = Touchscreen.current.primaryTouch.press.isPressed;
            isPressUp = Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
        }
        else if (Mouse.current != null)
        {
            pointerPosition = Mouse.current.position.ReadValue();
            isPressDown = Mouse.current.leftButton.wasPressedThisFrame;
            isPressed = Mouse.current.leftButton.isPressed;
            isPressUp = Mouse.current.leftButton.wasReleasedThisFrame;
        }

        if (isPressDown)
        {
            if (_cam != null)
                _dragOrigin = _cam.ScreenToWorldPoint(pointerPosition);
            else if (Camera.main != null)
                _dragOrigin = Camera.main.ScreenToWorldPoint(pointerPosition);
                
            _isDragging = true;
        }
        else if (isPressed && _isDragging)
        {
            Vector3 currentPos = Vector3.zero;
            if (_cam != null)
                currentPos = _cam.ScreenToWorldPoint(pointerPosition);
            else if (Camera.main != null)
                currentPos = Camera.main.ScreenToWorldPoint(pointerPosition);

            Vector3 difference = _dragOrigin - currentPos;
            transform.position += new Vector3(difference.x, difference.y, 0f);
        }
        else if (isPressUp)
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
