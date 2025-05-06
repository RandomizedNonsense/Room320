using UnityEngine;

public class FakeWizbowo : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;  // The projectile prefab fired by fake Wizbowo
    [SerializeField] Transform firePoint;          // The point from which the projectiles will be fired

    [SerializeField] float projectileSpeed = 10f;  // Speed of the projectile
    [SerializeField] float fireRate = 0.5f;        // Fire rate (time between projectiles)
    [SerializeField] float lifespan = 5f;          // Lifespan of the fake Wizbowo before disappearing

    private GameObject player;             // Reference to the player's transform

    private void Start()
    {
        // Find the player object and get the transform (assuming the player has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");

        // Fire projectiles at regular intervals
        InvokeRepeating("FireRegularProjectiles", 0f, fireRate);

        // Destroy the fake Wizbowo after its lifespan ends
        Destroy(gameObject, lifespan);
    }

    public void FireRegularProjectiles()
    {
        if (player != null && projectilePrefab != null && firePoint != null)
        {
            Vector2 directionToPlayer = (player.transform.position - firePoint.transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;

            WizbowoBullet projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<WizbowoBullet>();
            projectile.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Debug.LogError("Player not found or projectile/firePoint missing!");
        }
    }
}
