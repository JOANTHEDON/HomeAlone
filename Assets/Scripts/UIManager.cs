using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]private GameObject _upGradeButton;
    [SerializeField]private Transform _doorUpgradeSpawnPoint;
    [SerializeField]private Transform _cradleUpgradeSpawnPoint;
    [SerializeField]private int[] _doorUpgradeLevels;
    [SerializeField]private int[] _cradleUpgradeLevels;
    private int doorCurrentLevel = 1;
    private int cradleCurrentLevel = 0;
    private bool isDoorupgradeShown = false;
    private bool isCradleUpgradeShown = false;

    private GameObject activeDoorUpgradeIcon;
    private DoorHealthManager doorHealthManager;

    private void Start()
    {
        doorHealthManager = FindAnyObjectByType<DoorHealthManager>();
    }

    public void ShowDoorUpgradeButton(int currentCoinCount)
    {
        if (_upGradeButton == null || doorHealthManager == null) return;
        if (doorCurrentLevel > _doorUpgradeLevels.Length) return;

        int nextLevelCost = _doorUpgradeLevels[doorCurrentLevel - 1];

        if (currentCoinCount >= nextLevelCost && !isDoorupgradeShown)
        {
            activeDoorUpgradeIcon = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            AnimateUpgradeButton(activeDoorUpgradeIcon.transform);

            UnityEngine.UI.Button btn = activeDoorUpgradeIcon.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(UpgradeDoor);
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
        if (doorCurrentLevel > _doorUpgradeLevels.Length || doorHealthManager == null) return;

        int cost = _doorUpgradeLevels[doorCurrentLevel - 1];
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

    public void ShowCradleUpgradeButton(int CurrentCoinCount)
    {
        if(_upGradeButton == null || isCradleUpgradeShown)return;
        if( CurrentCoinCount >= _cradleUpgradeLevels[0])
        {
            var UpgradePrefab = Instantiate(_upGradeButton, _cradleUpgradeSpawnPoint.position, Quaternion.identity);
            cradleCurrentLevel++;
            AnimateUpgradeButton(UpgradePrefab.transform);
            isCradleUpgradeShown = true;
            
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
        buttonTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);

        buttonTransform.DOScale(new Vector3(1.08f, 1.08f, 1.08f), 0.45f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(0.5f);
    }


}
