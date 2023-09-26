using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{
    public float speed;
    public bool directionLeft;
    public Rigidbody2D m_Rigidbody2D;
    public Transform parent;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController temp = FindObjectOfType<PlayerController>();

        if (GetComponent<possessionObject>())
        {
            if (collision.gameObject.tag == "Player" && !temp.controller._dashing)
            {
                if (!temp.controller.m_Settings.invincibility)
                    temp.onDeath();
                else
                    Destroy(this.gameObject);
            }

            else if (collision.gameObject.tag != "Player" && GetComponent<possessionObject>().active)
            {
                GetComponent<possessionObject>().dashOut();
                temp.onDeath();
            }

            else if (collision.gameObject.tag != "Player" && !GetComponent<possessionObject>().active)
            {
                if (!temp.controller.m_Settings.invincibility)
                    temp.onDeath();
                else
                    Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Minecart")
        {
            if (!temp.controller.m_Settings.invincibility)
                temp.onDeath();
            else
                Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


    }
}
