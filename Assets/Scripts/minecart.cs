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


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        m_Settings = FindObjectOfType<playerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(m_Settings.jump))
            {

            }
            else if (Input.GetKeyDown(m_Settings.dash))
            {
                exit();

                Invoke("resetPlayerChild", 0.2f);
            }
            else if (Input.GetKeyDown(m_Settings.left))
            {
                if (speed > 0f)
                    speed *= -1f;
            }
            else if (Input.GetKeyDown(m_Settings.right))
            {
                if (speed < 0f)
                    speed *= -1f;
            }
        }
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
        player.transform.parent = transform;

        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<Rigidbody2D>().simulated = false;
        player.GetComponent<CapsuleCollider2D>().isTrigger = true;



        active = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void exit()
    {
        player.transform.position = playerSpot.position;
        player.transform.parent = null;

        rb.simulated = false;
        player.GetComponent<Rigidbody2D>().simulated = true;

        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        player.GetComponent<CharacterController2D>().m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<CapsuleCollider2D>().isTrigger = false;

        player.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        active = false;

        GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<PlayerController>().canMove = true;
        player.GetComponent<CharacterController2D>().dashing(true);
        rb.simulated = true;
        rb.gravityScale = 0f;


    }
}
