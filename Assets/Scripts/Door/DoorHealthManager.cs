using UnityEngine;
using UnityEngine.UI;
public class DoorHealthManager : MonoBehaviour {
    [SerializeField] private Image healthBar;
    [SerializeField] private float healthAmount = 100f;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.Space)) { Heal(20); }
    }
    public void TakeDamage(float damage) {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount) {
        healthAmount += healthAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;

    }

}
