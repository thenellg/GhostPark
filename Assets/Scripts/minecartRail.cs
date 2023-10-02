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

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        effector = this.GetComponent<PlatformEffector2D>();
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
        }
    }

    void resetEffector()
    {
        effector.rotationalOffset = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.transform.parent == null)
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
