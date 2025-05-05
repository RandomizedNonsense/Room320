using UnityEngine;

public class FakeWizbowo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isFake = false;
    private PlayerScript playerScript;  // Reference to the player's PlayerScript
    private Animator animator;          // Reference to the Animator component

    // Add references for projectilePrefab and firePoint
    private GameObject projectilePrefab;
    private Transform firePoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();  // Get the PlayerScript component from the player object
        }
    }

    // Add a method to set the references
    public void SetReferences(GameObject prefab, Transform point)
    {
        projectilePrefab = prefab;
        firePoint = point;
    }

    public void SetAsFake()
    {
        isFake = true;
        Destroy(GetComponent<Collider2D>());  // Remove the collider from the fake Wizbowo
    }

    public void FireFakeProjectiles()
    {
        // This will fire projectiles like the real Wizbowo, but without damaging the player
        if (playerScript != null && projectilePrefab != null && firePoint != null)
        {
            Vector2 directionToPlayer = (playerScript.transform.position - transform.position).normalized;

            // Fire the projectile towards the player
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            rbProjectile.linearVelocity = directionToPlayer * 10f;  // Set velocity towards player

            // Ensure the projectile doesn't hurt the player (you can use Layer Mask or Tags)
            projectile.tag = "FakeProjectile";  // Mark the projectile as fake
        }
    }
}
