//This is where the player movement and fire controls are,
//the "PlayerMovement" is an auto generated file from Unity
//that is necessary for this to work properly.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementScript : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    public float moveSpeed = 5f;

    public PlayerMovement playerControls;
    Vector2 moveDirection = Vector2.zero;

    private InputAction move;
    private InputAction fire;

    private Vector2 mousePos;
    private float my;
    private float mx;

    //[SerializeField] private GameObject gunPivot;

    //public SpriteRenderer bulletSprite; //probably need to do some logic with these sprites like with the player but idk how

    [SerializeField] private float bulletSpeed = 5f;

    //Gun varialbes
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;
    private Rigidbody2D rb;
    private float fireTimer; //determines when enough time has passed to shoot again

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5; 
    private int currentHealth;
    private bool isDead = false;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerMovement();
        currentHealth = maxHealth;
    }

    void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        
    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (isDead) return;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f; //The -90f makes the "top of the player act as the front of the player"
        //gunPivot.transform.localRotation = Quaternion.Euler(0, 0, angle); //Dont ask me what this does
        
        if (Input.GetMouseButton(0) && fireTimer<= 0f){
            Fire();
            fireTimer = fireRate;
        }
        else{
            fireTimer -= Time.deltaTime;
        }


        moveDirection = move.ReadValue<Vector2>();
        //Debug.Log("We moved: "+ move.ReadValue<Vector2>());

        // Update sprite
        if (moveDirection == Vector2.zero) {
            return;
        }

        if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x)) {
            playerSprite.sprite = moveDirection.y > 0 ? upSprite : downSprite;
        } else {
            playerSprite.sprite = moveDirection.x > 0 ? rightSprite : leftSprite;
        }
    }

    private void FixedUpdate() {
        //rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y*moveSpeed);
        rb.linearVelocity = new Vector2(mx, my).normalized * bulletSpeed;

        rb.linearVelocity = moveDirection * moveSpeed;

    }

    private void Fire() {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
    }

    public void TakeDamage(int damage = 1) {
        currentHealth -= damage;
        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0 && !isDead) { // If health drops to 0, disable movement/shooting
            Die();
            OnDisable();
        }
    }

    private void Die() {
        Debug.Log("Player has died.");
        isDead = true;
    }




}