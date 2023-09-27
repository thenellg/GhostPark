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
    public Rigidbody2D rb;
    public float speed = 3f;
    public float m_JumpForce = 140f;
    public Transform initialParent;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 originalSpot;
    [Range(0, 45f)] public float maxRotate = 27f;

    public bool fanActive = false;
    bool exitRight = true;
    public Vector2 fanForce = new Vector2();

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
        initialParent = transform.parent;

        if (m_FacingRight == false)
            speed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationZ = Mathf.Clamp(rb.rotation, -maxRotate, maxRotate);
        rb.rotation = rotationZ;

        if (active)
        {
            if (Input.GetKeyDown(m_Settings.jump) && m_Grounded)
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
                    this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
                    m_FacingRight = true;
                }
            }
            else if (Input.GetAxis("Horizontal") > 0f)
            {
                if (speed < 0f)
                {
                    speed *= -1f;
                    this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);
                    m_FacingRight = false;
                }
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
        
        player.transform.parent = transform;

        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<PlayerController>().controller.PlayerAnim.SetTrigger("inMinecart");
        player.GetComponent<PlayerController>().controller.PlayerAnim.SetBool("isWalking", false);
        player.GetComponent<Rigidbody2D>().simulated = false;
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;

        transform.parent = null;

        active = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void exit()
    {
        player.transform.position = playerSpot.position;
        player.transform.parent = null;


        if(m_Grounded)
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        
        rb.simulated = false;
        player.GetComponent<Rigidbody2D>().simulated = true;

        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;

        player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        transform.parent = initialParent;

        active = false;

        speed = Mathf.Abs(speed);
        m_FacingRight = true;

        GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<PlayerController>().canMove = true;

        if(exitRight)
            player.GetComponent<CharacterController2D>().dashing(true, new Vector2(-1.25f, 1.25f));
        else
            player.GetComponent<CharacterController2D>().dashing(true, new Vector2(1.25f, 1.25f));

        rb.simulated = true;
        //rb.gravityScale = 0f;

        Invoke("resetTrigger", 0.1f);
    }

    void resetTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
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

        if (rb.gravityScale > 0)
            rb.AddForce(new Vector2(0, m_JumpForce));
        else
            rb.AddForce(new Vector2(0, -m_JumpForce));
    }

    public void fanSet(Vector2 fanVelocity)
    {
        fanForce = fanVelocity;
        fanActive = true;
    }

    public void fanDeset()
    {
        rb.AddForce(fanForce, ForceMode2D.Force);
        fanForce = Vector2.zero;
        fanActive = false;
    }

    public void resetCart()
    {
        rb.velocity = Vector2.zero;
        transform.position = originalSpot;
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
            if (active)
            {
                exit();
                player.GetComponent<PlayerController>().onDeath();
                resetPlayerChild();
            }

            resetCart();
        }
        else if (collision.tag == "endMinecart")
        {
            if (collision.transform.rotation.y == 180)
                exitRight = true;
            else
                exitRight = false;

            exit();

            Invoke("resetCart", 1f);
        }
    }
}
