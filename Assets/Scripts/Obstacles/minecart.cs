using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minecart : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public bool playerChild = false;
    public Transform playerSpot;
    public playerSettings m_Settings;

    [Header("Minecart General")]
    public bool active = false;
    public bool controllable = false;
    public bool constant = false;
    public Rigidbody2D rb;
    public float speed = 3f;
    float originalSpeed;
    public float m_JumpForce = 10f;
    public float m_springForce = 20f;
    public Transform initialParent;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 originalSpot;
    [Range(0, 45f)] public float maxRotate = 27f;

    public bool fanActive = false;
    public bool exitRight = true;
    public Vector2 fanForce = new Vector2();
    public bool forceOut = false;
    public bool canJump = true;

    float tempGravScale;
    [Header("Ground Check")]
    public bool m_Grounded = true;
    public bool m_FacingRight = true;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        m_Settings = FindObjectOfType<playerSettings>();
        tempGravScale = rb.gravityScale;
        originalSpot = transform.position;
        originalSpeed = speed;
        initialParent = transform.parent;

        if (m_FacingRight == false)
            speed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.rotation > maxRotate || rb.rotation < -maxRotate)
            rb.angularVelocity = 0f;

        float rotationZ = Mathf.Clamp(rb.rotation, -maxRotate, maxRotate);
        rb.rotation = rotationZ;

        if (active)
        {
            if (controllable)
            {
                if (Input.GetKeyDown(m_Settings.jump) && m_Grounded && canJump)
                {
                    playerJump();
                }
                else if (Input.GetKeyDown(m_Settings.dash))
                {
                    exit();

                    Invoke("resetPlayerChild", 0.2f);
                }
                else if (Input.GetAxis("Horizontal") < 0f)
                {
                    if (speed > 0f)
                    {
                        speed *= -1f;
                        this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        m_FacingRight = false;
                    }
                }
                else if (Input.GetAxis("Horizontal") > 0f)
                {
                    if (speed < 0f)
                    {
                        speed *= -1f;
                        this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        m_FacingRight = true;
                    }
                }
            }

            if (m_Grounded)
            {             
                if ((rb.rotation < -11 && m_FacingRight) ||
                    (rb.rotation > 11 && !m_FacingRight))
                {
                    if (speed > 0)
                        speed = originalSpeed + 0.5f;
                    else
                        speed = -originalSpeed - 0.5f;
                }
                else
                {
                    if (speed > 0)
                        speed = originalSpeed;
                    else
                        speed = -originalSpeed;
                }
                
            }
            else
            {
                if (speed > 0)
                    speed = originalSpeed;
                else
                    speed = -originalSpeed;
            }

            Vector2 targetVelocity = new Vector2(speed * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            if (fanActive)
            {
                if (m_Grounded)
                    rb.AddForce(fanForce);
                else
                    rb.AddForce(fanForce * 2);
            }

            //Keep object rotated up
        }
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !(colliders[i].gameObject.GetComponent<pushableObject>() && colliders[i].gameObject.GetComponent<pushableObject>().hidden == true))
            {
                /*
                if (playLandingSFX)
                {
                    this.GetComponent<AudioSource>().PlayOneShot(landingSFX);
                    playLandingSFX = false;
                }
                */

                if (colliders[i].isTrigger == false)
                    m_Grounded = true;


            }
        }

        //PlayerAnim.SetBool("isGrounded", m_Grounded);

    }

    void resetPlayerChild()
    {
        playerChild = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
        rb.simulated = true;
        rb.gravityScale = 1f;
    }

    public void enter()
    {
        playerChild = true;

        player.transform.position = playerSpot.position;

        if(player.GetComponent<CharacterController2D>().m_FacingRight != m_FacingRight)
            player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);

        player.GetComponent<CharacterController2D>().fromPossession = true;
        player.GetComponent<CharacterController2D>().fromPossessionDash = true;

        player.transform.parent = transform;

        player.GetComponent<PlayerController>().currentMinecart = this;
        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<PlayerController>().controller.PlayerAnim.SetTrigger("inMinecart");
        player.GetComponent<PlayerController>().controller.PlayerAnim.SetBool("isWalking", false);
        player.GetComponent<Rigidbody2D>().simulated = false;
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;

        transform.parent = null;
        controllable = true;

        active = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void exit(bool dead = false)
    {
        controllable = false;
        player.transform.position = playerSpot.position;
        player.transform.parent = null;


        if(!constant && m_Grounded)
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        
        rb.simulated = false;
        player.GetComponent<Rigidbody2D>().simulated = true;

        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;

        player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        transform.parent = initialParent;

        GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<PlayerController>().canMove = true;
        player.GetComponent<PlayerController>().currentMinecart = null;

        player.GetComponent<CharacterController2D>().fromPossession = true;
        player.GetComponent<CharacterController2D>().fromPossessionDash = true;

        if (forceOut && exitRight)
            player.GetComponent<CharacterController2D>().dashing(true, new Vector2(1.25f, 0.5f));
        else if (forceOut && !exitRight)
            player.GetComponent<CharacterController2D>().dashing(true, new Vector2(-1.25f, 0.5f));
        else if (dead)
            player.GetComponent<CharacterController2D>().dashing(true, new Vector2(0f, -0.1f));
        else
            player.GetComponent<CharacterController2D>().dashing(true);

        rb.simulated = true;
        if (!constant)
        {
            active = false;
            speed = Mathf.Abs(speed);
            m_FacingRight = true;
        }
        //rb.gravityScale = 0f;

        Invoke("resetTrigger", 0.1f);
    }

    void resetTrigger()
    {
        player.GetComponent<CharacterController2D>().dashVector = Vector2.zero;
        GetComponent<BoxCollider2D>().isTrigger = false;
        forceOut = false;
    }

    void playerJump()
    {
        if (rb.gravityScale > 0)
            rb.gravityScale = tempGravScale;
        else
            rb.gravityScale = -tempGravScale;

        //Debug.Log(new Vector2(0f, m_JumpForce));

        m_Grounded = false;

        rb.velocity = new Vector2(0, 0);
        if (speed > 0)
            speed -= 0.5F;
        else
            speed += 0.5f;


        if (rb.gravityScale > 0)
            rb.AddForce(new Vector2(0, m_JumpForce), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(0, -m_JumpForce), ForceMode2D.Impulse);

        Invoke("resetSpeed", 0.3f);    
    }

    void resetSpeed()
    {
        if (speed > 0)
            speed += 0.5F;
        else
            speed -= 0.5f;
    }

    public void fanSet(Vector2 fanVelocity)
    {
        fanForce = fanVelocity;
        fanActive = true;
    }

    public void fanDeset()
    {
        //rb.AddForce(fanForce, ForceMode2D.Force);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        fanForce = Vector2.zero;
        fanActive = false;
    }

    public void resetCart()
    {
        active = false;
        rb.velocity = Vector2.zero;
        transform.position = originalSpot;
        transform.parent = initialParent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            speed *= -1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {
            if (GetComponentInChildren<PlayerController>())
            {
                exit();
                player.GetComponent<PlayerController>().onDeath();
                resetPlayerChild();
            }

            resetCart();
        }
        else if (collision.tag == "camSwap")
        {
            collision.gameObject.GetComponent<cameraSwitch>().setEnterDirection(transform);
        }
        else if (collision.tag == "Spring" && transform.position.y > collision.transform.position.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, m_springForce), ForceMode2D.Impulse);
        }
        else if (collision.tag == "Coin")
        {
            int temp = collision.gameObject.GetComponent<Coin>().amount;
            FindObjectOfType<playerSettings>().totalCoins += temp;
            //controller.deadSFX(collision.gameObject.GetComponent<Coin>().soundEffect);
            collision.gameObject.GetComponent<Coin>().hideCoin();
        }
        else if (collision.tag == "endMinecart")
        {
            Debug.Log("Rotation: " + collision.transform.rotation.y);
            if (collision.transform.rotation.y == 1)
                exitRight = true;
            else
                exitRight = false;

            forceOut = true;

            if(GetComponentInChildren<PlayerController>())
                exit();

            Invoke("resetCart", 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "camSwap" && active)
        {
            collision.gameObject.GetComponent<cameraSwitch>().checkCamSwap(transform);
        }
    }
}
