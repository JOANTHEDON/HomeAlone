using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorHealthManager : MonoBehaviour {
    [SerializeField] private Image healthBar;
    [SerializeField] private DoorConfig doorConfig;
    [SerializeField] private TextMeshProUGUI levelText;

    private float maxHealth = 10f;

    private float currentHealth;
    private bool _isDoorBroken = false;
    private int currentLevel = 1;

    public bool IsDoorBroken => _isDoorBroken;

    private void Awake() {
        currentHealth = maxHealth;
        UpdateHealthBar();
        _isDoorBroken = false;
    }

    private void Start() {
        UpdateLevelText();
        InitializeHealthFromConfig();
    }

    private void InitializeHealthFromConfig() {
        if (doorConfig != null && doorConfig._doorLevelList != null) {
            DoorLevelInfo levelInfo = GetLevelInfo(currentLevel);
            if (levelInfo != null) {
                maxHealth = levelInfo.doorHealth;
                currentHealth = maxHealth;
                UpdateHealthBar();
            }
        }
    }

    public void UpgradeToLevel(int newLevel) {
        currentLevel = newLevel;
        UpdateLevelText();

        if (doorConfig != null && doorConfig._doorLevelList != null) {
            DoorLevelInfo levelInfo = GetLevelInfo(currentLevel);
            if (levelInfo != null) {
                maxHealth = levelInfo.doorHealth;
                currentHealth = maxHealth; // Heal door to full health on upgrade
                _isDoorBroken = false;
                UpdateHealthBar();
            }
        }
    }

    private DoorLevelInfo GetLevelInfo(int level) {
        if (doorConfig == null || doorConfig._doorLevelList == null) return null;
        foreach (var info in doorConfig._doorLevelList._doorLevelInfo) {
            if (info.level == level) return info;
        }
        return null;
    }

    private void UpdateLevelText() {
        if (levelText != null) {
            levelText.text = $"LEVEL {currentLevel}";
        }
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