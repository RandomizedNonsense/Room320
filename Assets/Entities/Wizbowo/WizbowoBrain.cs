using UnityEngine;
using System.Collections;
public class WizbowoBrain : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isAttacking = false;   // Determines whether or not Wizbowo is currenlty acting, or idling

    [Range(0f, 10f)]
    [SerializeField] float idleTime = 5f;   // How long Wizbowo idles before resuming attacks

    [Range(0f, 1f)]
    [SerializeField] float waveAttackOdds = 0.5f;   // The odds of choosing the "WandWave" attack over the "WandSlam" attack

    [SerializeField] GameObject projectilePrefab;  // The projectile prefab that Wizbowo fires
    [SerializeField] Transform firePoint;          // The position where the projectiles will be fired from

    private PlayerScript playerScript; // Reference to the player's PlayerScript
    private Animator animator; // Reference to the Animator component
    
     private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); //Initialize animator
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>(); // Get the PlayerScript component from the player object
        }

        StartCoroutine(IdleAndAttackRoutine()); //start the idle and attack routine
    }

    public void FireProjectile()
{
    if (playerScript != null && projectilePrefab != null && firePoint != null)
    {
        // Get the player's position from the PlayerScript
        Vector2 directionToPlayer = (playerScript.transform.position - transform.position).normalized;

        // Fire a projectile towards the player
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        rbProjectile.linearVelocity = directionToPlayer * 10f; // Set velocity towards player
    }
    else
    {
        Debug.LogError("Projectile Prefab or Fire Point not assigned!");
    }
}


     private void WandWave()
    {
        if (playerScript != null)
        {
            // Trigger the wave animation
            animator.SetTrigger("WaveTrigger"); // This triggers the wave animation

            // Get the player's position from the PlayerScript
            Vector2 directionToPlayer = (playerScript.transform.position - transform.position).normalized;

            // Fire multiple projectiles towards the player
            for (int i = 0; i < 5; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
                rbProjectile.linearVelocity = directionToPlayer * 10f; // Set velocity towards player
            }
        }
        else
        {
            Debug.LogError("Player's PlayerScript is not found!");
        }

        isAttacking = false;
    }

    void WandSlam() {
        // Notes: in this attack, Wizbowo slams his wand against the ground twice before disappearing, shortly reappearing with two or three duplicates
        if (animator != null)
        {
            // Trigger the slam animation
            animator.SetTrigger("SlamTrigger"); // This triggers the slam animation
        }

        isAttacking = false;
    }

    private IEnumerator IdleAndAttackRoutine()
    {
        while (true)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                // Decide attack type based on odds
                if (Random.value < waveAttackOdds)
                {
                    // Trigger the idle animation (if needed)
                    animator.SetTrigger("IdleTrigger"); // This triggers the idle animation
                    WandWave();
                }
                else
                {
                    WandSlam();
                }

                yield return new WaitForSeconds(idleTime); // Wait for idle time after attack
            }

            yield return null;
        }
    }
}
