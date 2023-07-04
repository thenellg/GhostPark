using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class possessionObject : MonoBehaviour
{
    public PlayerController player;
    public Vector3 scale;

    public bool active = false;

    public bool showingArrow = false;
    public GameObject arrow;

    public bool through = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (active)
        {
            //dashVector = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            //dashVector = dashVector.normalized;

            if (Input.GetButtonDown("Dash"))
            {
                dashOut();
            }
        }
    }

    public void setUp()
    {
        if (GetComponent<BoxCollider2D>())
            GetComponent<BoxCollider2D>().isTrigger = true;
        else if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true ;

        scale = player.transform.localScale;

        //disabling normal player activity
        player.controller.m_Rigidbody2D.velocity = Vector2.zero;
        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;


        player.canMove = false;
        player.transform.position = transform.position;
        player.transform.localScale = Vector3.zero;
        //setting this object to active controller
        active = true;

        if (through)
            dashOut();
    }

    public void dashOut()
    {
        player.canMove = true;
        active = false;

        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.transform.localScale = scale;
        player.transform.parent = null;
        
        player.controller.dashing();
        player.controller.fromPossession = true;
        //player.controller.Move(0f, false, true, false, false);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GetComponent<BoxCollider2D>())
            GetComponent<BoxCollider2D>().isTrigger = false;
        else if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
