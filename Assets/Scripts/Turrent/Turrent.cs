using UnityEngine;
using DG.Tweening;

public class Turret : MonoBehaviour {
    [Header("Turret Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float fireRate = 1f; // 1 bullet per second
    [SerializeField] private GameObject projectilePrefab; // Assign TurrentProjectile Prefab
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask ghostLayer;
    [SerializeField] private TurretConfig _turretConfig;

    [Header("Upgrade Settings")]
    private int currentLevel = 1;
    private float projectileDamage = 10f; // Default baseline damage
    private GameObject activeUpgradeIcon;
    private CoinManager coinManager;
    private UIManager uiManager;

    private GhostHealth currentTarget;
    private float fireCooldown = 0f;

    private void Start() {
        coinManager = FindAnyObjectByType<CoinManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        
        // Initialize level 1 stats from config if available
        TurretLevelInfo level1Info = GetLevelInfo(1);
        if (level1Info != null) {
            ApplyUpgradedStats(level1Info);
        }
    }

    private void Update() {
        FindTarget();

        fireCooldown -= Time.deltaTime;

        // Fire 1 bullet per second if Ghost is within attack range
        if (currentTarget != null && fireCooldown <= 0f) {
            Shoot();
            fireCooldown = 1f / fireRate; // Cooldown = 1 second
        }

        HandleUpgradeUI();
    }

    private void Shoot() {
        if (projectilePrefab == null) return;
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        GameObject bulletGO = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        TurrentProjectile bullet = bulletGO.GetComponent<TurrentProjectile>();

        if (bullet != null) {
            bullet.SetDamage(projectileDamage);
            if (currentTarget != null) {
                bullet.Seek(currentTarget.transform);
            }
        }
    }

    private void FindTarget() {
        if (currentTarget != null) {
            float distance = Vector2.Distance(transform.position, currentTarget.transform.position);
            if (distance <= attackRange) return;
            currentTarget = null;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, ghostLayer);
        float closestDistance = Mathf.Infinity;
        GhostHealth closestGhost = null;

        foreach (var col in hitColliders) {
            GhostHealth ghost = col.GetComponent<GhostHealth>();
            if (ghost != null) {
                float dist = Vector2.Distance(transform.position, ghost.transform.position);
                if (dist < closestDistance) {
                    closestDistance = dist;
                    closestGhost = ghost;
                }
            }
        }

        currentTarget = closestGhost;
    }

    private void HandleUpgradeUI() {
        if (_turretConfig == null || coinManager == null || uiManager == null) return;

        TurretLevelInfo nextLevelInfo = GetLevelInfo(currentLevel + 1);

        if (nextLevelInfo == null) {
            if (activeUpgradeIcon != null) Destroy(activeUpgradeIcon);
            return;
        }

        bool canAfford = coinManager.CurrentCoinCount >= nextLevelInfo.LevelCoinUpgrade;

        if (canAfford && activeUpgradeIcon == null) {
            activeUpgradeIcon = uiManager.SpawnTurretUpgradeButton(transform.position, UpgradeTurret);
        } else if (!canAfford && activeUpgradeIcon != null) {
            Destroy(activeUpgradeIcon);
        }
    }

    private void OnDestroy() {
        if (activeUpgradeIcon != null) {
            Destroy(activeUpgradeIcon);
        }
    }

    private TurretLevelInfo GetLevelInfo(int level) {
        if (_turretConfig == null || _turretConfig._turretLevelList == null) return null;
        foreach (var info in _turretConfig._turretLevelList.TurretLevelInfos) {
            if (info.Level == level) return info;
        }
        return null;
    }

    private void UpgradeTurret() {
        TurretLevelInfo nextLevelInfo = GetLevelInfo(currentLevel + 1);
        if (nextLevelInfo == null || coinManager == null) return;

        if (coinManager.SpendCoins((int)nextLevelInfo.LevelCoinUpgrade)) {
            currentLevel++;
            ApplyUpgradedStats(nextLevelInfo);

            if (activeUpgradeIcon != null) {
                Destroy(activeUpgradeIcon);
            }
        }
    }

    private void ApplyUpgradedStats(TurretLevelInfo levelInfo) {
        if (levelInfo.AttackRange > 0f) attackRange = levelInfo.AttackRange;
        if (levelInfo.FireRate > 0f) fireRate = levelInfo.FireRate;
        if (levelInfo.ProjectileDamage > 0f) projectileDamage = levelInfo.ProjectileDamage;
        Debug.Log($"Turret Upgraded to Level {levelInfo.Level}! Range: {attackRange}, Fire Rate: {fireRate}, Damage: {projectileDamage}");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}