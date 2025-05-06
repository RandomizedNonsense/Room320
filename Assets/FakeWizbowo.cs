using UnityEngine;

public class FakeWizbowo : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;  // The projectile prefab fired by fake Wizbowo
    [SerializeField] Transform firePoint;          // The point from which the projectiles will be fired

    [SerializeField] float projectileSpeed = 10f;  // Speed of the projectile
    [SerializeField] float fireRate = 0.5f;        // Fire rate (time between projectiles)
    [SerializeField] float lifespan = 5f;          // Lifespan of the fake Wizbowo before disappearing

    private Transform playerTransform;             // Reference to the player's transform

    private void Start()
    {
        // Find the player object and get the transform (assuming the player has the "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        // Ensure SetReferences is called to set projectilePrefab and firePoint
        if (projectilePrefab != null && firePoint != null)
        {
            SetReferences(projectilePrefab, firePoint);
        }
        else
        {
            Debug.LogError("Projectile Prefab or FirePoint is missing in FakeWizbowo.");
        }

        // Fire projectiles at regular intervals
        InvokeRepeating("FireRegularProjectiles", 0f, fireRate);

        // Destroy the fake Wizbowo after its lifespan ends
        Destroy(gameObject, lifespan);
    }


    // Method to set references for projectile prefab and fire point
    public void SetReferences(GameObject projectilePrefab, Transform firePoint)
    {
        this.projectilePrefab = projectilePrefab;
        this.firePoint = firePoint;

        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile Prefab or Fire Point not assigned in FakeWizbowo!");
        }
        else
        {
            Debug.Log("FakeWizbowo references set successfully.");
        }
    }


    public void FireRegularProjectiles()
    {
        if (playerTransform != null && projectilePrefab != null && firePoint != null)
        {
            // Fire a regular projectile (this will hurt the player)
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

            // Fire the projectile towards the player
            Vector2 directionToPlayer = (playerTransform.position - firePoint.position).normalized;
            rbProjectile.linearVelocity = directionToPlayer * projectileSpeed;
        }
        else
        {
            Debug.LogError("Player not found or projectile/firePoint missing!");
        }
    }
}
