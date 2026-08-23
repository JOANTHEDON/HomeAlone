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
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private CradleConfig cradleConfig;
    [SerializeField] private TextMeshProUGUI _cradleLevelText;

    private bool _startCoinSpawn = false;
    public bool StartCoinSpawn {
        get => _startCoinSpawn;
        set => _startCoinSpawn = value;
    }

    public int CurrentCoinCount => currentCoinCount;

    private int currentCoinCount = 0;
    private CoinScript _coin;
    private int cradleCurrentLevel = 1;

    public void Start() {
        if (_textUI == null) return;
        if (_uiManager == null) return;
        _coin = Instantiate(_coinPrefab);
        _coin.gameObject.SetActive(false);
        UpdateSpawnTimeFromConfig();
        UpdateCradleLevelText();
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

    public void Update() {
        _uiManager.ShowDoorUpgradeButton(currentCoinCount);
        _uiManager.ShowCradleUpgradeButton(currentCoinCount);
    }

    public void StopCoinSpawn() {
        _startCoinSpawn = false;
    }
    public bool SpendCoins(int amount) {
        if (currentCoinCount >= amount) {
            currentCoinCount -= amount;
            if (_textUI != null) _textUI.text = currentCoinCount.ToString();
            return true;
        }
        return false;
    }

    public void UpgradeCradle(int newLevel) {
        cradleCurrentLevel = newLevel;
        UpdateSpawnTimeFromConfig();
        UpdateCradleLevelText();
    }

    private void UpdateSpawnTimeFromConfig() {
        if (cradleConfig != null && cradleConfig._cradleLevelList != null) {
            CradleLevelInfo levelInfo = GetCradleLevelInfo(cradleCurrentLevel);
            if (levelInfo != null) {
                _coinSpawnTime = levelInfo.CoinSpawnRate;
                Debug.Log($"Cradle Upgraded to Level {cradleCurrentLevel}! Coin Spawn Time is now: {_coinSpawnTime}s");
            }
        }
    }

    private CradleLevelInfo GetCradleLevelInfo(int level) {
        if (cradleConfig == null || cradleConfig._cradleLevelList == null) return null;
        if (cradleConfig._cradleLevelList._cradleLevelInfo != null) {
            foreach (var info in cradleConfig._cradleLevelList._cradleLevelInfo) {
                if (info.Level == level) return info;
            }
        }
        return null;
    }

    private void UpdateCradleLevelText() {
        if (_cradleLevelText != null) {
            _cradleLevelText.text = $"LEVEL {cradleCurrentLevel}";
        }
    }

    public int GetCradleUpgradeCost(int targetLevel) {
        CradleLevelInfo info = GetCradleLevelInfo(targetLevel);
        return info != null ? (int)info.LevelCoinUpgrade : -1;
    }
}