using UnityEngine;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private CradleController _cradleController;
    [SerializeField] private DoorController _door;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private Transform _ghostSpawnPoint;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private TextMeshProUGUI _uiText;
    [SerializeField] private float _gameStartTime = 10f;

    private bool _hasGhostSpawned = false;
    private bool _playerHidden = false;
    private GameObject Player;

    private void Start()
    {
        if (_playerPrefab == null)
        {
            Debug.LogWarning("GameController: Player prefab is not assigned.");
            return;
        }

        Player = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
        InitializeCameraFollow(Player.transform);

        if (_uiText != null)
        {
            _uiText.gameObject.SetActive(true);
        }

        StartCoroutine(StartCountDown());
    }

    private IEnumerator StartCountDown()
    {
        for (int i = (int)_gameStartTime; i >= 0; i--)
        {
            if (_uiText != null)
                _uiText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        if (_uiText != null)
            _uiText.gameObject.SetActive(false);

        if (_cradleController != null && _cradleController.HasClosed && !_hasGhostSpawned)
        {
            SpawnGhost();
            _hasGhostSpawned = true;
        }
    }

    private void Update()
    {
        if (_cradleController != null && _cradleController.HasClosed && !_playerHidden)
        {
            DisablePlayer();
            _playerHidden = true;
        }
    }

    private void DisablePlayer()
    {
        if (Player != null)
            Player.SetActive(false);
    }

    private void SpawnGhost()
    {
        if (_ghostPrefab == null) return;
        Instantiate(_ghostPrefab, _ghostSpawnPoint.position, Quaternion.identity);
    }

    private void InitializeCameraFollow(Transform target)
    {
        if (_cameraFollow == null)
        {
            Debug.LogWarning("GameController: CameraFollow reference is not assigned.");
            return;
        }

        _cameraFollow.SetTarget(target);
    }
}