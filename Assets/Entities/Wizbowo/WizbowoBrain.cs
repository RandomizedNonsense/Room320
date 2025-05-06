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

    [SerializeField] GameObject fakeWizbowoPrefab;  // Reference to the Fake Wizbowo prefab to spawn as clone
    
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

                WizbowoBullet projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<WizbowoBullet>();
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

    private IEnumerator WandSlam()
    {
        Debug.Log("WandSlam started!");
        // Trigger the slam animation
        animator.SetTrigger("SlamTrigger"); // This triggers the slam animation

        // Create two fake Wizbowo clones
        GameObject fakeWizbowo1 = Instantiate(fakeWizbowoPrefab, transform.position + new Vector3(-5f, -3f, 0f), Quaternion.identity);
        GameObject fakeWizbowo2 = Instantiate(fakeWizbowoPrefab, transform.position + new Vector3(5f, -3f, 0f), Quaternion.identity);

        // Set the references for both fake Wizbowos
        FakeWizbowo fakeWizbowoScript1 = fakeWizbowo1.GetComponent<FakeWizbowo>();
        FakeWizbowo fakeWizbowoScript2 = fakeWizbowo2.GetComponent<FakeWizbowo>();

        // Set the projectile-related properties on the fake Wizbowos (same as real Wizbowo)
        fakeWizbowoScript1.SetReferences(projectilePrefab, firePoint);
        fakeWizbowoScript2.SetReferences(projectilePrefab, firePoint);

        fakeWizbowo1.SetActive(true);
        fakeWizbowo2.SetActive(true);

        // Have fake Wizbowos fire projectiles that interact with the player (they'll hurt the player)
        fakeWizbowoScript1.FireRegularProjectiles();
        fakeWizbowoScript2.FireRegularProjectiles();


        // Control how long the fake Wizbowos appear in the scene (duration is adjustable)
        float fakeWizbowoDuration = 3f; // Adjust this value to control how long they stay
        Debug.Log("Fake Wizbowos will stay for: " + fakeWizbowoDuration + " seconds.");

        // Wait for the set duration before cleaning up the fake Wizbowos
        yield return new WaitForSeconds(fakeWizbowoDuration);  // Duration of the fake Wizbowo clones' existence

        
        // Destroy fake Wizbowos after the attack is done
        Destroy(fakeWizbowo1);
        Destroy(fakeWizbowo2);

        isAttacking = false;
        Debug.Log("WandSlam completed!");
    }


    private IEnumerator IdleAndAttackRoutine()
{
    while (true)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Debugging log to check Random value and waveAttackOdds
            Debug.Log("Random.value: " + Random.value);
            Debug.Log("waveAttackOdds: " + waveAttackOdds);

            // Decide attack type based on odds
            if (Random.value < waveAttackOdds)
            {
                Debug.Log("WandWave is selected");
                yield return StartCoroutine(WandWave());
            }
            else
            {
                Debug.Log("WandSlam is selected");
                yield return StartCoroutine(WandSlam()); // Make sure WandSlam is being triggered
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
