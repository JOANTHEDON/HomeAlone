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

    public void ShowDoorUpgradeButton(int currentCoinCount)
    {
        if(_upGradeButton == null || isDoorupgradeShown) return;
        if(currentCoinCount >= _doorUpgradeLevels[0])
        {
            var UpgradePrefab = Instantiate(_upGradeButton, _doorUpgradeSpawnPoint.position, Quaternion.identity);
            doorCurrentLevel++;
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
            isCradleUpgradeShown = true;
            
        }
    }


}
