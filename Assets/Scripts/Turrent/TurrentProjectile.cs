using UnityEngine;

public class TurrentProjectile : MonoBehaviour {
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 9f;

    private Transform target;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        // Destroy projectile automatically after 9 seconds if it doesn't hit anything
        Destroy(gameObject, lifetime);

        // Ignore collisions between projectile and turret colliders
        Collider2D projCollider = GetComponent<Collider2D>();
        if (projCollider != null) {
            Turret parentTurret = GetComponentInParent<Turret>();
            if (parentTurret != null) {
                Collider2D turretCol = parentTurret.GetComponent<Collider2D>();
                if (turretCol != null) {
                    Physics2D.IgnoreCollision(projCollider, turretCol);
                }
            }
        }
    }

    public void Seek(Transform targetTransform) {
        target = targetTransform;
    }

    public void SetDamage(float newDamage) {
        damage = newDamage;
    }

    private void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position);
        float distanceThisFrame = speed * Time.deltaTime;

        // Move projectile towards target
        if (rb != null && !rb.isKinematic) {
            rb.linearVelocity = dir.normalized * speed;
        } else {
            transform.position += dir.normalized * distanceThisFrame;
        }

        // Distance fallback check to guarantee hit when projectile arrives close to ghost
        if (dir.magnitude <= 0.4f) {
            HitGhost(target.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        HitGhost(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        HitGhost(collision.gameObject);
    }

    private void HitGhost(GameObject hitObject) {
        if (hitObject == null) return;
        
        GhostHealth health = hitObject.GetComponentInParent<GhostHealth>();
        if (health != null) {
            health.TakeDamage(damage);
            Destroy(gameObject); // Destroy bullet immediately on hit
        }
    }
}
