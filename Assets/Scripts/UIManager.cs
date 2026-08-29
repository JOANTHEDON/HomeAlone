using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]private GameObject _upGradeButton;
    [SerializeField]private Transform _doorUpgradeSpawnPoint;
    [SerializeField]private Transform _cradleUpgradeSpawnPoint;
    [SerializeField]private GameObject _upgradePopUpUI;
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

    private void Awake()
    {
        _upgradePopUpUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        doorHealthManager = FindAnyObjectByType<DoorHealthManager>();
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
            _upgradeDoor = true;

            UnityEngine.UI.Button btn = activeDoorUpgradeIcon.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(DoorUpgradePopUp);
                //_upgradePopUpUI.SetActive(true);
            }

            isDoorupgradeShown = true;
        }
        else if (currentCoinCount < nextLevelCost && isDoorupgradeShown)
        {
            if (activeDoorUpgradeIcon != null) Destroy(activeDoorUpgradeIcon);
            isDoorupgradeShown = false;
        }
    }

    private void DoorUpgradePopUp()
    {
        _upgradeDoor = true;
        _upgradePopUpUI.SetActive(true);
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

            UnityEngine.UI.Button btn = activeCradleUpgradeIcon.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(UpgradeCradle);
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

    public GameObject SpawnTurretUpgradeButton(Vector3 position, UnityEngine.Events.UnityAction onClickAction)
    {
        if (_upGradeButton == null) return null;
        var upgradeButtonInstance = Instantiate(_upGradeButton, position, Quaternion.identity);
        AnimateUpgradeButton(upgradeButtonInstance.transform);
        
        UnityEngine.UI.Button btn = upgradeButtonInstance.GetComponentInChildren<UnityEngine.UI.Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(onClickAction);
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

    public void OnYesButtonClicked()
    {
        if (_upgradeDoor)
        {
            UpgradeDoor();
            _upgradeDoor = false;
        }

        if (_upgradeCradle)
        {
            
        }
        if (_upgradeTurret)
        {
            
        }
        
        _upgradePopUpUI.SetActive(false);

    }

    public void OnNoButtonClicked()
    {
        _upgradeDoor = false;
        _upgradePopUpUI.SetActive(false);
    }


}
