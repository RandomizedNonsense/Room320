using UnityEngine;

//copied code from bullet.cs for now, might change later

public class WizbowoBullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;


    [Range(1,10)]
    [SerializeField] private float lifeTime = 3f; //here just in case the bullet doesn't get destroyed when it hits the wall
    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // For now, destroy the bullet on any collision.
        Destroy(gameObject);
    }


    //add OnTriggerEnter2D if you want to to be destroyed when it collides with an enemy
    // void OnTriggerEnter2D(Collider2D collision)
    // {
        
    // }
}
