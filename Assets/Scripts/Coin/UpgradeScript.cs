using UnityEngine;
using TMPro;

public class UpgradeScript : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _upgradeCost;

    public void ShowUpgradeCost(int UpgradeCost)
    {
        if(_upgradeCost == null) return;

        _upgradeCost.text = UpgradeCost.ToString();
    }
}
