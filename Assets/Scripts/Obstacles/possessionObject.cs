using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class possessionObject : MonoBehaviour
{
    public PlayerController player;
    public playerSettings settings;
    public Vector3 scale = new Vector3 (5.772182f, 5.772182f, 5.772182f);

    public bool active = false;

    public bool showingArrow = false;
    public GameObject arrow;

    public bool through = false;
    public bool camSwap = false;

    public bool ifEndLevel = false;
    public GameObject hideObjects;
    //Add in an animator

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        settings = FindObjectOfType<playerSettings>();
    }

    // Update is called once per frame
    void Update()
    {

        if (active)
        {
            //dashVector = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            //dashVector = dashVector.normalized;

            if (Input.GetKeyDown(settings.dash))
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

        if (camSwap)
            GetComponent<cameraSwitch>().setEnterDirection(player.transform);

        //disabling normal player activity
        player.controller.m_Rigidbody2D.velocity = Vector2.zero;
        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        player.controller.lastPossession = this;

        player.inObject = true;
        player.canMove = false;
        player.transform.position = transform.position;
        player.transform.localScale = Vector3.one * 0.01f;
        player.controller.fanActive = false;
        player.controller.fanForce = Vector2.zero;
        //setting this object to active controller
        active = true;

        if (through)
            dashOut();
    }

    public void dashOut()
    {
        player.canMove = true;
        player.inObject = false;
        active = false;

        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
        player.controller.m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.transform.localScale = scale;
        player.transform.parent = null;
        
        player.controller.dashing();
        player.controller.fromPossession = true;
        player.controller.fromPossessionDash = true;
        player.controller.fanActive = false;
        player.controller.fanForce = Vector2.zero;
        //player.controller.Move(0f, false, true, false, false);


        if (camSwap && player.controller.dashVector.y < 0.05 && player.controller.dashVector.y > -0.05)
            GetComponent<cameraSwitch>().checkCamSwap(player.transform, true);
        else if (camSwap && player.controller.dashVector.y != 0)
            GetComponent<cameraSwitch>().checkCamSwap(player.transform);

        if (ifEndLevel)
        {
            Time.timeScale = 0.2f;
            hideObjects.SetActive(false);
            Invoke("dialogue", 0.5f);
        }
    }

    void dialogue()
    {
        Time.timeScale = 1f;
        GetComponent<forceDialogueTrigger>().startDialogue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GetComponent<BoxCollider2D>())
            GetComponent<BoxCollider2D>().isTrigger = false;
        else if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
