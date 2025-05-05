using UnityEngine;

public class WizbowoBullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;


    [Range(1,10)]
    [SerializeField] private float lifeTime = 3f; //how long the bullet will be alive
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }



    //add OnTriggerEnter2D if you want to to be destroyed when it collides with an enemy
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FakeProjectile"))
        {
            // Fake projectiles don't hurt the player
            return;
        }
         if (collision.CompareTag("Player"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
