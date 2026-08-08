using System.Collections;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private float _coinSpawnTime = 1f;
    [SerializeField] private CoinScript _coinPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _coinDuration = 0.5f;

    private bool _startCoinSpawn = false;
    public bool StartCoinSpawn {
        get => _startCoinSpawn;
        set => _startCoinSpawn = value;
    }

    private int currentCoinCount = 0;
    private CoinScript _coin;

    public void Start() {
        if (_textUI == null) return;
        _coin = Instantiate(_coinPrefab);
        _coin.gameObject.SetActive(false);
        StartCoroutine(CoinSpawnCoroutine());
    }

    IEnumerator CoinSpawnCoroutine() {
        while (true) {
            if (_startCoinSpawn) {
                yield return new WaitForSeconds(_coinSpawnTime);
                _coin.Initialize(_spawnPoint.position, _targetPoint.position, _coinDuration);
                currentCoinCount++;
                _textUI.text = currentCoinCount.ToString();
                Debug.Log("coin increased");
            } else {
                yield return null;
            }
        }
    }
}