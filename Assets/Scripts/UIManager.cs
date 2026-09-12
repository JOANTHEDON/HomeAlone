using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]private GameObject _upGradeButton;
    [SerializeField]private Transform _doorUpgradeSpawnPoint;
    [SerializeField]private Transform _cradleUpgradeSpawnPoint;
    [SerializeField]private GameObject _upgradePopUpUI;

    [Header("Zoom UI Settings")]
    [SerializeField]private GameObject _zoomUIPanel;
    [SerializeField]private UnityEngine.UI.Slider _zoomSlider;
    [SerializeField]private CameraFollow _cameraFollow;
    [SerializeField]private GameObject _locationButton;

    [Header("Settings Panel")]
    [SerializeField]private GameObject _settingsPanel;
    [SerializeField]private GameObject _settingsButton;

    [Header("Music Part")]
    [SerializeField]private Sprite _volumeOff;
    [SerializeField]private Sprite _volumeOn;
    [SerializeField]private Image _soundButtonImage;


    private int doorCurrentLevel = 1;
    private int cradleCurrentLevel = 1;
    private bool isDoorupgradeShown = false;
    private bool isCradleUpgradeShown = false;

    private GameObject activeDoorUpgradeIcon;
    private GameObject activeCradleUpgradeIcon;
    private DoorHealthManager doorHealthManager;
    


    private bool _upgradeDoor = false;
    private bool _upgradeCradle= false;
    private bool _upgradeTurret= false;
    private UnityEngine.Events.UnityAction _activeTurretUpgradeAction;

    private void Awake()
    {
        if (_upgradePopUpUI != null) _upgradePopUpUI.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(false);
        if (_settingsButton != null) _settingsButton.SetActive(true);
    }

    private void Start()
    {
        // Sync music button sprite with actual Audio Manager state
        if (_soundButtonImage != null && GameAudioManager.Instance != null)
        {
            _soundButtonImage.sprite = GameAudioManager.Instance.IsMuted ? _volumeOff : _volumeOn;
        }
        doorHealthManager = FindAnyObjectByType<DoorHealthManager>();
        if (_cameraFollow == null) _cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (_zoomUIPanel != null) _zoomUIPanel.SetActive(false);
        if(_locationButton != null) _locationButton.SetActive(false);


        if (_zoomSlider != null)
        {
            _zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);
        }
    }

    public void EnableLocationBtn()
    {
        _locationButton.SetActive(true);
    }

    public void ShowZoomUIPanel(bool show)
    {
        if (_zoomUIPanel != null)
        {
            _zoomUIPanel.SetActive(show);
        }

        if (show && _zoomSlider != null)
        {
            _zoomSlider.value = 0.5f; // Center slider by default
        }
    }

    public void OnZoomSliderValueChanged(float value)
    {
        if (_cameraFollow == null) _cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (_cameraFollow != null)
        {
            _cameraFollow.SetZoomRatio(value);
        }
    }

    public void OnZoomInButtonClicked()
    {
        if (_cameraFollow != null)
        {
            _cameraFollow.ZoomIn();
        }
    }

    public void OnZoomOutButtonClicked()
    {
        if (_cameraFollow != null)
        {
            _cameraFollow.ZoomOut();
        }
    }

    public void ShowDoorUpgradeButton(int currentCoinCount)
    {
        if (_upGradeButton == null || doorHealthManager == null) return;

        int nextLevelCost = doorHealthManager.GetUpgradeCost(doorCurrentLevel + 1);
        if (nextLevelCost == -1) // Max level reached
        {
            if (activeDoorUpgradeIcon != null) Destroy(activeDoorUpgradeIcon);
            return;
        }

        if (currentCoinCount >= nextLevelCost && !isDoorupgradeShown)
        {
            activeDoorUpgradeIcon = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            AnimateUpgradeButton(activeDoorUpgradeIcon.transform);
            UpgradeScript upgradeScript = activeDoorUpgradeIcon.GetComponent<UpgradeScript>();
            if (upgradeScript != null)
            {
                upgradeScript.ShowUpgradeCost(nextLevelCost);
            }
            UnityEngine.UI.Button btn = activeDoorUpgradeIcon.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OpenDoorUpgradePopUp);
            }

            isDoorupgradeShown = true;
        }
        else if (currentCoinCount < nextLevelCost && isDoorupgradeShown)
        {
            if (activeDoorUpgradeIcon != null) Destroy(activeDoorUpgradeIcon);
            isDoorupgradeShown = false;
        }
    }

    private void UpgradeDoor()
    {
        if (doorHealthManager == null) return;

        int cost = doorHealthManager.GetUpgradeCost(doorCurrentLevel + 1);
        if (cost == -1) return;

        CoinManager coinManager = FindAnyObjectByType<CoinManager>();
        if (coinManager != null && coinManager.SpendCoins(cost))
        {
            doorCurrentLevel++;
            doorHealthManager.UpgradeToLevel(doorCurrentLevel);

            if (activeDoorUpgradeIcon != null)
            {
                Destroy(activeDoorUpgradeIcon);
            }
            isDoorupgradeShown = false;
        }
    }

    public void ShowCradleUpgradeButton(int currentCoinCount)
    {
        if (_upGradeButton == null) return;

        CoinManager coinManager = FindAnyObjectByType<CoinManager>();
        if (coinManager == null) return;

        int nextLevelCost = coinManager.GetCradleUpgradeCost(cradleCurrentLevel + 1);
        if (nextLevelCost == -1) // Max level reached
        {
            if (activeCradleUpgradeIcon != null) Destroy(activeCradleUpgradeIcon);
            return;
        }

        if (currentCoinCount >= nextLevelCost && !isCradleUpgradeShown)
        {
            activeCradleUpgradeIcon = Instantiate(_upGradeButton, _cradleUpgradeSpawnPoint.position, Quaternion.identity);
            AnimateUpgradeButton(activeCradleUpgradeIcon.transform);


            UpgradeScript upgradeScript = activeCradleUpgradeIcon.GetComponent<UpgradeScript>();
                if (upgradeScript != null)
                {
                    upgradeScript.ShowUpgradeCost(nextLevelCost);
                }
            UnityEngine.UI.Button btn = activeCradleUpgradeIcon.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OpenCradleUpgradePopUp);
            }

            isCradleUpgradeShown = true;
        }
        else if (currentCoinCount < nextLevelCost && isCradleUpgradeShown)
        {
            if (activeCradleUpgradeIcon != null) Destroy(activeCradleUpgradeIcon);
            isCradleUpgradeShown = false;
        }
    }

    private void UpgradeCradle()
    {
        CoinManager coinManager = FindAnyObjectByType<CoinManager>();
        if (coinManager == null) return;

        int cost = coinManager.GetCradleUpgradeCost(cradleCurrentLevel + 1);
        if (cost == -1) return;

        if (coinManager.SpendCoins(cost))
        {
            cradleCurrentLevel++;
            coinManager.UpgradeCradle(cradleCurrentLevel);

            if (activeCradleUpgradeIcon != null)
            {
                Destroy(activeCradleUpgradeIcon);
            }
            isCradleUpgradeShown = false;
        }
    }

    public GameObject SpawnTurretUpgradeButton(Vector3 position, int cost, UnityEngine.Events.UnityAction onClickAction)
    {
        if (_upGradeButton == null) return null;
        var upgradeButtonInstance = Instantiate(_upGradeButton, position, Quaternion.identity);
        AnimateUpgradeButton(upgradeButtonInstance.transform);
        
        // Add this block:
        UpgradeScript upgradeScript = upgradeButtonInstance.GetComponent<UpgradeScript>();
        if (upgradeScript != null)
        {
            upgradeScript.ShowUpgradeCost(cost);
        }
        
        UnityEngine.UI.Button btn = upgradeButtonInstance.GetComponentInChildren<UnityEngine.UI.Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => OpenTurretUpgradePopUp(onClickAction));
        }
        return upgradeButtonInstance;
    }

    private void AnimateUpgradeButton(Transform buttonTransform)
    {
        Canvas canvas = buttonTransform.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }

        buttonTransform.localScale = Vector3.one;
        buttonTransform.DOScale(Vector3.one, 0.25f)
            .SetEase(Ease.OutBack)
            .SetTarget(buttonTransform);

        buttonTransform.DOScale(new Vector3(1.08f, 1.08f, 1.08f), 0.45f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(0.5f)
            .SetTarget(buttonTransform);
    }

    private void OpenDoorUpgradePopUp()
    {
        _upgradeDoor = true;
        _upgradePopUpUI.SetActive(true);
    }

    private void OpenCradleUpgradePopUp()
    {
        _upgradeCradle = true;
        _upgradePopUpUI.SetActive(true);
    }

    private void OpenTurretUpgradePopUp(UnityEngine.Events.UnityAction confirmAction)
    {
        _activeTurretUpgradeAction = confirmAction;
        _upgradeTurret = true;
        _upgradePopUpUI.SetActive(true);
    }

    public void OnYesButtonClicked()
    {
        if (_upgradeDoor)
        {
            UpgradeDoor();
            _upgradeDoor = false;
        }
        else if (_upgradeCradle)
        {
            UpgradeCradle();
            _upgradeCradle = false;
        }
        else if (_upgradeTurret)
        {
            _activeTurretUpgradeAction?.Invoke();
            _activeTurretUpgradeAction = null;
            _upgradeTurret = false;
        }
        
        _upgradePopUpUI.SetActive(false);
    }

    public void OnNoButtonClicked()
    {
        _upgradeDoor = false;
        _upgradeCradle = false;
        _upgradeTurret = false;
        _activeTurretUpgradeAction = null;
        _upgradePopUpUI.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        _settingsButton.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _settingsButton.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void ExitGamePlay()
    {
        SceneManager.LoadScene("HomeMenuScene");
    }

    public void ToggleMusicUI()
    {
        if (GameAudioManager.Instance == null) return;

        // Toggle the music
        GameAudioManager.Instance.ToggleMusic();

        // Update the sprite
        if (_soundButtonImage != null)
        {
            _soundButtonImage.sprite = GameAudioManager.Instance.IsMuted ? _volumeOff : _volumeOn;
        }
    }
}
