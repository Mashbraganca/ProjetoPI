using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float hp;
    private float mp;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 200.0f;
    [SerializeField]
    private float jumpforce = 10.0f;
    private Vector2 velocidadeFinal;
    private PlayerControls playerInputActions;
    private bool jumped = false;
    [SerializeField]
    private bool grounded = true;
    private Vector2 movementInput = Vector2.zero;

    // Start is called before the first frame update
    void Awake()
    {
        hp = 100;
        mp = 100;

        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerControls();
        playerInputActions.Player.Enable();


    }

    public void OnHit1(InputAction.CallbackContext context)
    {

    }

    public void OnHit2(InputAction.CallbackContext context)
    {

    }

    public void OnDash(InputAction.CallbackContext context)
    {

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    void FixedUpdate()
    {
        velocidadeFinal = movementInput * speed * Time.fixedDeltaTime;
        rb.velocity = new Vector2(velocidadeFinal.x, rb.velocity.y);
        if (jumped && grounded)
        {
            grounded = false;
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
        }
        

    }
}

