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
    [SerializeField] float waveAttackDelay = 0.1f;  // How long we should wait between each projectile being fired

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


     private IEnumerator WandWave()
     {
        if (playerScript != null)
        {
            // Trigger the wave animation
            animator.SetTrigger("WaveTrigger"); // This triggers the wave animation

            // Fire multiple projectiles towards the player
            for (int i = 0; i < 5; i++)
            {
                // Get the player's position from the PlayerScript
                Vector2 directionToPlayer = (playerScript.transform.position - firePoint.transform.position).normalized;
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;

                Bullet projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
                projectile.transform.localRotation = Quaternion.Euler(0, 0, angle);

                yield return new WaitForSeconds(waveAttackDelay);
            }

            // Wait for animation to finish
            while (isAttacking) {
                yield return new WaitForEndOfFrame();
            }
            isAttacking = true;
        }
        else
        {
            Debug.LogError("Player's PlayerScript is not found!");
        }

        isAttacking = false;
    }

    private IEnumerator WandSlam() {
        // I don't think we need to complete this attack. It's probably doable for this sprint, but it's likely very complicated

        // Notes: in this attack, Wizbowo slams his wand against the ground twice before disappearing, shortly reappearing with two or three duplicates
        if (animator != null)
        {
            // Trigger the slam animation
            animator.SetTrigger("SlamTrigger"); // This triggers the slam animation
        }

        // Create two fake Wizbowo clones
        GameObject fakeWizbowo1 = Instantiate(gameObject, transform.position, Quaternion.identity);
        GameObject fakeWizbowo2 = Instantiate(gameObject, transform.position, Quaternion.identity);

        // Remove colliders for fake Wizbowos
        Destroy(fakeWizbowo1.GetComponent<Collider2D>());
        Destroy(fakeWizbowo2.GetComponent<Collider2D>());

        // Get the FakeWizbowo script on the fake Wizbowos
        FakeWizbowo fakeWizbowoScript1 = fakeWizbowo1.GetComponent<FakeWizbowo>();
        FakeWizbowo fakeWizbowoScript2 = fakeWizbowo2.GetComponent<FakeWizbowo>();

        // Set the references for both fake Wizbowos
        fakeWizbowoScript1.SetReferences(projectilePrefab, firePoint);
        fakeWizbowoScript2.SetReferences(projectilePrefab, firePoint);

        // Have fake Wizbowos fire projectiles but not harm the player
        fakeWizbowoScript1.FireFakeProjectiles();
        fakeWizbowoScript2.FireFakeProjectiles();

        // Wait for a short duration before cleaning up the fake Wizbowos
        yield return new WaitForSeconds(3f);  // Duration of the fake Wizbowo clones' existence

        // Destroy fake Wizbowos after the attack is done
        Destroy(fakeWizbowo1);
        Destroy(fakeWizbowo2);

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
                    yield return StartCoroutine(WandWave());
                }
                else
                {
                    yield return StartCoroutine(WandSlam());
                }

                animator.SetTrigger("IdleTrigger");

                yield return new WaitForSeconds(idleTime); // Wait for idle time after attack
            }

            yield return null;
        }
    }

    public void FinishAttack() {
        isAttacking = false;
    }
}
