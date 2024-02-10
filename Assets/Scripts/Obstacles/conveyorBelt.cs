using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorBelt : MonoBehaviour
{
    public Rigidbody2D player;
    public float speed;
    public bool active = true;

    public List<Rigidbody2D> objects = new List<Rigidbody2D>();

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<CharacterController2D>().GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            foreach (Rigidbody2D m_Rigidbody2D in objects)
                m_Rigidbody2D.AddForce(new Vector2(speed, 0f), ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
            objects.Add(collision.gameObject.GetComponent<Rigidbody2D>());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            objects.Remove(collision.gameObject.GetComponent<Rigidbody2D>());

            if(collision.gameObject.tag != "Player")
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed/2, 0f), ForceMode2D.Impulse);
        }
    }

    public void flipActive()
    {
        active = !active;
    }
}
