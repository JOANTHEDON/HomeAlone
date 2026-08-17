using UnityEngine;
using UnityEngine.UI;

public class DoorHealthManager : MonoBehaviour {
    [SerializeField] private Image healthBar;
    [SerializeField] private float maxHealth = 10f;

    private float currentHealth;
    private bool _isDoorBroken= false;
    public bool IsDoorBroken => _isDoorBroken;

    private void Awake() {
        currentHealth = maxHealth;
        UpdateHealthBar();
        _isDoorBroken= false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            _isDoorBroken = true;

            DoorController doorController = GetComponentInParent<DoorController>();
            if (doorController == null)
                doorController = GetComponentInChildren<DoorController>();

            if (doorController != null)
                doorController.OpenDoor();
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