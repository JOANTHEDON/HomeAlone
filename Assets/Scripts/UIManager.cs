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
    private int doorCurrentLevel = 0;
    private int cradleCurrentLevel = 0;
    private bool isDoorupgradeShown= false;
    private bool isCradleUpgradeShown = false;

    public void ShowDoorUpgradeButton(int currentCoinCount)
    {
        if(_upGradeButton == null || isDoorupgradeShown) return;
        if(currentCoinCount >= _doorUpgradeLevels[0])
        {
            var UpgradePrefab = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            doorCurrentLevel++;
            AnimateUpgradeButton(UpgradePrefab.transform);
            isDoorupgradeShown = true;
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

    private void AnimateUpgradeButton(Transform buttonTransform)
    {
        buttonTransform.localScale = Vector3.one;
        buttonTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);

        buttonTransform.DOScale(new Vector3(1.08f, 1.08f, 1.08f), 0.45f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(0.5f);
    }


}
