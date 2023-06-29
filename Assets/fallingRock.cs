using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingRock : MonoBehaviour
{
    public bool fatal = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find("Spawning Hazards").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && fatal)
        {
            FindObjectOfType<PlayerController>().onDeath();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {

            Destroy(this.gameObject);
        }
    }

}
