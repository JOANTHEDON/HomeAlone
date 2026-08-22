using UnityEngine;

public class Turret : MonoBehaviour {
    [Header("Turret Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float fireRate = 1f; // 1 bullet per second
    [SerializeField] private GameObject projectilePrefab; // Assign TurrentProjectile Prefab
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask ghostLayer;

    private GhostHealth currentTarget;
    private float fireCooldown = 0f;

    private void Update() {
        FindTarget();

        fireCooldown -= Time.deltaTime;

        // Fire 1 bullet per second if Ghost is within attack range
        if (currentTarget != null && fireCooldown <= 0f) {
            Shoot();
            fireCooldown = 1f / fireRate; // Cooldown = 1 second
        }
    }

    private void Shoot() {
        if (projectilePrefab == null) return;
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        GameObject bulletGO = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        TurrentProjectile bullet = bulletGO.GetComponent<TurrentProjectile>();

        if (bullet != null && currentTarget != null) {
            bullet.Seek(currentTarget.transform);
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

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}