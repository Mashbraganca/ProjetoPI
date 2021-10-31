using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    private float hp;
    private float maxhp=100;
    private float mp;
    private float maxmp=100;

    public int playerid;

    [SerializeField]
    public HUDBars HUD;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float speed = 200.0f;
    [SerializeField]

    private float dashspeed = 400.0f;

    //Dash duration logics
    private float dashtime = 0;
    private float dashduration = 0.2f;

    //Dash frequency logics
    private float dashreload = 0;
    private float dashfrequency = 1;


    //HitsFrequency
    private float hitreload = 0;
    private float hitfrequency = 4;

    //Hit1 logics
    private Vector3 hit1offset = new Vector3(1, 0, 0);
    private float hit1range = 0.5f;
    private float hit1damage = 5;
    private float hit1MPregen = 10;

    //Hit2 logics
    private Vector3 hit2offset = new Vector3(0, 0, 0);
    private float hit2range = 3f;
    private float hit2damage = 25;
    private float hit2MPcost = 40;

    //Fall Damage
    private float falldamage = 10;

    //EndGame Event
    private UnityEvent<int> deathEvent;
    private UnityEvent pauseEvent;

    //Jump logics
    [SerializeField]
    private float jump1force = 10.0f;
    [SerializeField]
    private float jump2force = 3.0f;
    private bool jumped = false;
    private bool grounded = true;
    private int jumpCount = 0;
    private int jumpMax = 2;
    [SerializeField]
    PhysicsMaterial2D basic;
    [SerializeField]
    PhysicsMaterial2D nofriction;


    private Vector2 velocidadeFinal;
    private PlayerControls playerInputActions;

    private int front = -1;
    [SerializeField]

    private Vector2 movementInput = Vector2.zero;

    [SerializeField]
    LayerMask hitlayers;

    [SerializeField]
    public GameObject SpawnPoint;

    bool inCollisionWithPlayer;

    private CapsuleCollider2D capsuleCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Awake()
    {
        deathEvent = new UnityEvent<int>();
        pauseEvent = new UnityEvent();
    }

    void Start()
    {
        hp = 100;
        mp = 50;

        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerControls();
        playerInputActions.Player.Enable();

        HUD.SetHPMaxValue(maxhp);
        HUD.SetMPMaxValue(maxmp);
        HUD.SetMPValue(mp);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();


        inCollisionWithPlayer = false;

    }

    public void OnHit1(InputAction.CallbackContext context)
    {
       if(context.performed) 
        if (Time.time >= hitreload)
        {
            hitreload = Time.time + 1f / hitfrequency;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + front * hit1offset, hit1range, hitlayers);
            foreach (Collider2D hit in hitEnemies)
            {
                if(hit.gameObject != this.gameObject)
                {
                    hit.gameObject.GetComponent<PlayerController>().TakeHit(hit1damage);
                    mp += hit1MPregen;
                    if (mp > maxmp) mp = maxmp;
                    HUD.SetMPValue(mp);
                }
            }
        }

    }

    public void OnHit2(InputAction.CallbackContext context)
    {

        if (context.performed)
            if (Time.time >= hitreload && mp >= hit2MPcost)
            {
                hitreload = Time.time + 1f / hitfrequency;
                mp -= hit2MPcost;
                HUD.SetMPValue(mp);
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + front * hit2offset, hit2range, hitlayers);
                foreach (Collider2D hit in hitEnemies)
                {
                    if (hit.gameObject != this.gameObject)
                    {
                        hit.gameObject.GetComponent<PlayerController>().TakeHit(hit2damage);
                    }
                }
            }



    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (dashtime <= 0 && Time.time >= dashreload)
        {
            dashtime = dashduration;
            dashreload = Time.time + 1f / dashfrequency; 
            gameObject.layer = 6;

        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
            movementInput = context.ReadValue<Vector2>();
        if (movementInput.x > 0)
        {
            front = 1;
            spriteRenderer.flipX = false;
            
        }
        else if (movementInput.x < 0)
        {
            front = -1;
            spriteRenderer.flipX = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {


        jumped = context.action.triggered;

        if (jumped && grounded &&jumpCount < jumpMax)
        {
            //rb.sharedMaterial = nofriction;
           // capsuleCollider.sharedMaterial = nofriction;
            animator.SetBool("jump", true);
            if(jumpCount == 0)
            {
                print(jump1force);
                jumpCount += 1;
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * jump1force, ForceMode2D.Impulse);
            }
            else
            {
                print(jump2force);
                jumpCount += 1;
                grounded = false;
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * jump2force, ForceMode2D.Impulse);
            }

        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            pauseEvent.Invoke();
        }
    }

    public void TakeHit(float damage)
    {
        animator.Play("hit");
        hp -= damage;
        HUD.SetHPValue(hp);
        if (hp <= 0) Death();
    }
    private void Death()
    {
        
        deathEvent.Invoke(playerid);
      
    }
    void FixedUpdate()
    {
        if(dashtime <= 0 && gameObject.layer != 7)
        {
            gameObject.layer = 7;
        }
        
        if(dashtime > 0)
        {
            velocidadeFinal = new Vector2(front,0) * dashspeed * Time.fixedDeltaTime;
            dashtime -= Time.fixedDeltaTime;
            if (rb.velocity.y < -0.2)
            {
                animator.SetBool("falling", true);

            }
            rb.velocity = new Vector2(velocidadeFinal.x, 0);
        }
        else
        {
            if (!inCollisionWithPlayer)
            {
                velocidadeFinal = movementInput * speed * Time.fixedDeltaTime;
                rb.velocity = new Vector2(velocidadeFinal.x, rb.velocity.y);
            }
            else
            {
                rb.AddForce(new Vector2(0, -1f) * 1, ForceMode2D.Impulse);
            }


        }

        animator.SetFloat("speed", Mathf.Abs(movementInput.x));
        animator.SetFloat("yspeed",rb.velocity.y);

        if(rb.velocity.y < -0.2)
        {
            animator.SetBool("falling", true);
            

        }







    }



    private void Update()
    {
        if (transform.position.y < -15f)
        {
            transform.position = SpawnPoint.transform.position;
            TakeHit(falldamage);
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
            animator.SetBool("jump", false);
            animator.SetBool("falling", false);
            jumpCount = 0;
        }

       else if(collision.gameObject.CompareTag("Platform"))
        {
            if(collision.GetContact(0).normal.y > 0)
            {
                rb.sharedMaterial = basic;
                capsuleCollider.sharedMaterial = basic;
                grounded = true;            
                animator.SetBool("jump", false);
                animator.SetBool("falling", false);
                jumpCount = 0;
                
            }

            
        }

        else if(collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            if (collision.GetContact(0).normal.y > 0)
            {
                // rb.sharedMaterial = basic;
                // capsuleCollider.sharedMaterial = basic;
                inCollisionWithPlayer = true;
                grounded = true;
                animator.SetBool("jump", false);
                animator.SetBool("falling", false);
                jumpCount = 0;

            }
        }
       

    }



    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.sharedMaterial = nofriction;
            capsuleCollider.sharedMaterial = nofriction;
        }

        else if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {

                inCollisionWithPlayer = false;
        }


        }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + hit1offset, hit1range);
        Gizmos.DrawWireSphere(transform.position + hit2offset, hit2range);
    }

    public void registerDeathEvent(UnityAction<int> action)
    {
        deathEvent.AddListener(action);
    }
    public void registerPauseEvent(UnityAction action)
    {
        pauseEvent.AddListener(action);
    }
}

