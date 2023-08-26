using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private Vector3 originSpot;
    private Transform parent;
    public float speed = 5;

    public CharacterController2D player;
    public bool following;
    public Transform followSpot;
    bool end = true;

    private Animator keyAnim;
    public bool inPosition = false;
    public Door door = null;

    private void Start()
    {
        keyAnim = this.gameObject.GetComponent<Animator>();
        originSpot = this.transform.position;
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (following && Vector2.Distance(transform.position, followSpot.position) > 0)
            transform.position = Vector2.MoveTowards(transform.position, followSpot.position, speed * Time.deltaTime);
        
        if (inPosition)
        {
            Color temp = this.GetComponent<SpriteRenderer>().color;
            temp.a = temp.a - 0.1f;
            this.GetComponent<SpriteRenderer>().color = temp;
        }
    }

    private void FixedUpdate()
    {
        if (door && transform.position.x == door.transform.position.x && transform.position.y == door.transform.position.y && door.GetComponent<Door>().isLocked == true)
        {
            keyAnim.SetTrigger("Fade");
            inPosition = true;
            door.GetComponent<Door>().unlocked();
            this.transform.parent = FindObjectOfType<PlayerController>().transform;
            Invoke("boom", 1.35f);
            end = false;
        }
    }

    public void startDoor(Door newDoor)
    {
        door = newDoor;
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, newDoor.transform.position.z);
    }

    void boom()
    {
        //player.endLevel();
        this.gameObject.SetActive(false);
    }

    public void setFollow()
    {
        following = true;
    }

    public void resetKey()
    {
        speed = 5;
        this.transform.position = originSpot;
        this.transform.parent = parent;
        this.GetComponent<BoxCollider2D>().enabled = true;
        door = null;
        following = false;
        inPosition = false;
    }

    public void resetParent()
    {
        this.transform.parent = parent;
    }
}