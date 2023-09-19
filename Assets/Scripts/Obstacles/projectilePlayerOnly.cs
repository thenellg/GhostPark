using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectilePlayerOnly : MonoBehaviour
{
    public float speed;
    public bool directionLeft;
    public Rigidbody2D m_Rigidbody2D;
    public Transform parent;

    public float timer = 0f;
    public float maxTime = 60f;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        transform.parent = GameObject.Find("Spawning Hazards").transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody2D.AddForce(new Vector2(speed, 0));

        if(timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController temp = FindObjectOfType<PlayerController>();
        if (collision.tag == "Player" || collision.tag == "Minecart")
        {
            if((collision.transform.position.y > transform.position.y && temp.controller.m_Rigidbody2D.gravityScale > 0) || (collision.transform.position.y < transform.position.y && temp.controller.m_Rigidbody2D.gravityScale < 0))
            {
                temp.controller.playerJump();
                Destroy(this.gameObject);
            }
            else
            {
                if (!temp.controller.m_Settings.invincibility)
                    temp.onDeath();
                else
                    Destroy(this.gameObject);
            }
        }
    }


}
