using UnityEngine;
using UnityEngine.UI;

public class DoorHealthManager : MonoBehaviour {
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth = 10f;

    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0) {
            Debug.Log("Door Destroyed!");
        }
    }

    public void Heal(float healingAmount) {
        currentHealth += healingAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}