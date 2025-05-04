using UnityEngine;

public class Bullet : MonoBehaviour
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
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<WizbowoBrain>();
            if (enemy != null)
            {
                enemy.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
            }
        }
        // Hurt Player
        else if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<MovementScript>();
            if (player != null)
            {
                player.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Destroy bullet on collision with wall or any tagged object
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        
    }
}
