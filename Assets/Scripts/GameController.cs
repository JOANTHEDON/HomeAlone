using Unity.Cinemachine;

using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]private GameObject _playerPrefab;
    [SerializeField]private Transform _spawnPoint;
    [SerializeField]private CinemachineCamera _cameraFollow;
    [SerializeField]private CoinManager _coinManager;
    [SerializeField]private CradleController _cradleController;
    [SerializeField]private DoorController _door;
    private bool _disablePlayer = false;
    GameObject Player;

    public void Awake()
    {
        if(_playerPrefab == null)return;
        if(_cameraFollow == null)return;
        _disablePlayer = false;
        
        
    }

    public void Start()
    {
        Player =Instantiate(_playerPrefab,_spawnPoint.position, Quaternion.identity);
        InitializeCameraFollow(Player.transform);
    }

    private void InitializeCameraFollow(Transform target)
    {   
        _cameraFollow.Follow = target;
        
    }

    private void DisablePlayer()
    {
        Player.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_cradleController.HasClosed == true)
        {
            _coinManager.StartCoinSpawn = true;
            DisablePlayer();
        }
        
    }
    


}
