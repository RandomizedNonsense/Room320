//This is where the player movement and fire controls are,
//the "PlayerMovement" is an auto generated file from Unity
//that is necessary for this to work properly.


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MovementScript : MonoBehaviour
{
    //public GameObject playerSprite;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public PlayerMovement playerControls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerMovement();
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        Debug.Log("We moved: "+ move.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y*moveSpeed);
    }

    private void Fire(InputAction.CallbackContext context){
        Debug.Log("We fired.");
    }
}
