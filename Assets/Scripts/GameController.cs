using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private CradleController _cradleController;
    [SerializeField] private DoorController _door;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField]private Transform _ghostSpawnPoint;
    [SerializeField]private GameObject Canvas;
    [SerializeField]private TextMeshProUGUI _uiText;
    [SerializeField]private float _gameStartTime = 10f;
    private bool _shouldGameStart= false;
    
    
    
    private bool _hasghostSpawned = false;
    private bool _disablePlayer = false;
    private GameObject Player;

    private void Awake()
    {
        _disablePlayer = false;

    }

    

    private void Start()
    {
        if (_playerPrefab == null)
        {
            Debug.LogWarning("GameController: Player prefab is not assigned.");
            return;
        }
        _uiText.gameObject.SetActive(true);
        Player = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
        InitializeCameraFollow(Player.transform);
        StartCoroutine(StartCountDown());
        
    }

    private IEnumerator StartCountDown()
    {
        for (int i = (int)_gameStartTime; i >= 0; i--)
        {
            _uiText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        _uiText.gameObject.SetActive(false);
        _shouldGameStart = true;
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

    private void DisablePlayer()
    {
        Player.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_cradleController.HasClosed == true && _hasghostSpawned == false && _shouldGameStart == true )
        {
            _coinManager.StartCoinSpawn = true;
            DisablePlayer();
            SpawnGhost();
            _hasghostSpawned = true;
        }
        
    }

    private void SpawnGhost()
    {
        if(_ghostPrefab == null) return;
        GameObject ghost = Instantiate(_ghostPrefab, _ghostSpawnPoint.position, Quaternion.identity);
        
    }
    


}
