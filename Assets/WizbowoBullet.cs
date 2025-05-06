using UnityEngine;

public class WizbowoBullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

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



    //Currently is simply destroyed when it collides with the enemy or an obstacle, we need to add damage to it
    void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().TakeDamage(damage);

            Destroy(gameObject);
        }
        //else if (collision.CompareTag("Obstacle"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
