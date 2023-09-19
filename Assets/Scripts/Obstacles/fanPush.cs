using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanPush : MonoBehaviour
{
    public bool active = false;
    public bool interaction = false;
    public Transform fanOrigin;
    public CharacterController2D player;
    
    public float fanIntensity = 0.05f;
    public float fanModifier = 6f;
    public float initialIntensity;
    public bool horizontal = true;
    public bool inverted = false;


    private void Start()
    {
        player = FindObjectOfType<CharacterController2D>();
        initialIntensity = fanIntensity;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(player.m_Settings.hold))
            {
                interaction = true;
                fanIntensity = initialIntensity * fanModifier;
                player.m_Rigidbody2D.velocity = new Vector2(player.m_Rigidbody2D.velocity.x, player.m_Rigidbody2D.velocity.y / 2); 
                player.fanSet(setFanDirection());
            }
            else if (Input.GetKeyUp(player.m_Settings.hold))
            {
                interaction = false;
            }

            if (!interaction)
            {
                fanIntensity = initialIntensity;
                player.fanSet(setFanDirection());
            }

        }
    }

    Vector2 setFanDirection()
    {
        Vector2 fanDirection = Vector2.zero;
        if (horizontal)
            fanDirection.x = fanIntensity;
        else
            fanDirection.y = fanIntensity;

        if (inverted)
            fanDirection = fanDirection * -1;

        return fanDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 fanDirection = setFanDirection();

        if (collision.tag == "Player")
        {
            player.fanSet(fanDirection);
            active = true;
        }
        else if (collision.tag == "Minecart")
            collision.GetComponent<minecart>().fanSet(fanDirection);
        else if (collision.tag == "Box")
            collision.GetComponent<pushableObject>().fanSet(fanDirection);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CharacterController2D>().fanDeset();
            active = false;
            fanIntensity = initialIntensity;
        }
        else if (collision.tag == "Minecart")
        {
            collision.GetComponent<minecart>().fanDeset();
        }
        else if (collision.tag == "Box")
        {
            collision.GetComponent<pushableObject>().fanDeset();
            if (horizontal)
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
