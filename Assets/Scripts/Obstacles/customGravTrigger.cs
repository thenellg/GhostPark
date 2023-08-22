using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customGravTrigger : MonoBehaviour
{
    //reverse Gravity Trigger
    public bool setActive = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            gravFlip(collision.gameObject);

            setActive = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            gravFlip(collision.gameObject);

            setActive = false;
        }
    }


    public void gravFlip(GameObject flippin)
    {

        Rigidbody2D m_Rigidbody2D = flippin.GetComponent<Rigidbody2D>();

        Debug.Log(m_Rigidbody2D.gameObject.name);

        m_Rigidbody2D.gravityScale *= -1f;
        Debug.Log(m_Rigidbody2D.gravityScale);
        Vector3 theScale = flippin.transform.localScale;
        theScale.y *= -1;
        flippin.transform.localScale = theScale;
    }
}
