using Unity.VisualScripting;
using UnityEngine;

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

    public void ShowDoorUpgradeButton(int currentCoinCount, bool isUpgradeShown)
    {
        if(_upGradeButton == null)return;
        isUpgradeShown = isDoorupgradeShown;
        if(currentCoinCount >= _doorUpgradeLevels[doorCurrentLevel] && !isDoorupgradeShown)
        {
            var UpgradePrefab = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            doorCurrentLevel++;
            isDoorupgradeShown = false;
        }
    }

    public void ShowCradleUpgradeButton(int CurrentCoinCount, bool isUpgradeShown)
    {
        if(_upGradeButton == null)return;
        isUpgradeShown = isCradleUpgradeShown;
        if( CurrentCoinCount >= _cradleUpgradeLevels[cradleCurrentLevel] && !isCradleUpgradeShown)
        {
            var UpgradePrefab = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            cradleCurrentLevel++;
            isCradleUpgradeShown = false;
        }
    }


}
