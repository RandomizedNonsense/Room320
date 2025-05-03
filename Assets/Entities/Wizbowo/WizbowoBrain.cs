using UnityEngine;

public class WizbowoBrain : MonoBehaviour
{
    private bool isAttacking = false;   // Determines whether or not Wizbowo is currenlty acting, or idling

    [Range(0f, 10f)]
    [SerializeField] float idleTime = 5f;   // How long Wizbowo idles before resuming attacks

    [Range(0f, 1f)]
    [SerializeField] float waveAttackOdds = 0.5f;   // The odds of choosing the "WandWave" attack over the "WandSlam" attack

    void Start()
    {
        
    }

    void Update()
    {
        
    }



    void WandWave() {
        // Notes: in this attack, Wizbowo waves his wand around, spawning database projectiles towards the player

    }

    void WandSlam() {
        // Notes: in this attack, Wizbowo slams his wand against the ground twice before disappearing, shortly reappearing with two or three duplicates
    }
}
