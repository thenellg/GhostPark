using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minecartRail : MonoBehaviour
{
    private PlatformEffector2D effector;
    //private Animator PlayerAnim;
    public PlayerController player;
    public bool canDrop = false;
    public bool active = false;
    public bool ifRB = false;
    Rigidbody2D rb;
    public float maxRotation = 0;
    float initialRotation;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        effector = this.GetComponent<PlatformEffector2D>();
        if (ifRB)
        {
            rb = GetComponent<Rigidbody2D>();
            initialRotation = rb.rotation;
        }
    }

    void Update()
    {
        if (active && player.currentMinecart != null)
        {
            if (Input.GetAxis("Vertical") < 0 && canDrop)
            {
                player.currentMinecart.canJump = false;
                if (Input.GetKeyDown(player.controller.m_Settings.jump))
                {
                    Debug.Log("drop");
                    player.currentMinecart.rb.AddForce(new Vector2(0f, -8f), ForceMode2D.Impulse);
                    effector.rotationalOffset = 180f;
                    Invoke("resetEffector", 0.5f);
                }
            }
            else
            {
                player.currentMinecart.canJump = true;
                resetEffector();
            }

            if (ifRB)
            {
                if(rb.rotation < -maxRotation || rb.rotation > maxRotation)
                {
                    rb.angularVelocity = 0;
                    rb.freezeRotation = true;
                }
            }
        }
    }

    void resetEffector()
    {
        effector.rotationalOffset = 0f;
    }

    public void resetRotation()
    {
        rb.rotation = initialRotation;
        rb.angularVelocity = 0;
        rb.freezeRotation = false;
        rb.angularVelocity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.parent == null && !collision.gameObject.GetComponent<CharacterController2D>().m_Grounded)
            collision.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;

        if (collision.gameObject.tag == "Minecart")
            active = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Minecart")
            active = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.transform.parent == null)
            collision.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
    }
}
