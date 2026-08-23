using UnityEngine;
using UnityEngine.UI;

public class GhostHealth : MonoBehaviour {
    [Header("UI Reference")]
    [SerializeField] private Image healthBarImage; // Drag your UI Image here (Image Type must be 'Filled')

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 20f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercentage => maxHealth > 0 ? currentHealth / maxHealth : 0f;

    private void Awake() {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        Debug.Log($"{gameObject.name} took {damageAmount} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(float healAmount) {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthBarImage != null) {
            // Fills from 1.0 (full) down to 0.0 (empty)
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die() {
        Debug.Log($"{gameObject.name} was defeated!");
        Destroy(gameObject);
    }
}