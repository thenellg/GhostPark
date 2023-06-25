using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileShoot : MonoBehaviour
{
    public BoxCollider2D triggerArea;
    public GameObject projectile;
    public GameObject originPoint;

    public int interval;
    public bool intervalActive = false;
    public bool directionLeft = true;
    public bool active = false;

    public bool possessable = false;
    private int count = 0;
    public int possessionInterval;

    // Start is called before the first 1frame update
    void Start()
    {
        triggerArea = GetComponent<BoxCollider2D>();
    }

    public void instantiateProjectile()
    {
        GameObject _projectile = Instantiate(projectile);
        //Set necessities for fireball

        if (possessable)
        {
            count++;
            if(count == possessionInterval)
            {
                projectile.AddComponent<possessionObject>();
                count = 0;
            }

        }

        Invoke("resetInterval", interval);
    }

    void resetInterval()
    {
        if (active)
            instantiateProjectile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            active = true;
            Invoke("instantiateProjectile",1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            active = false;
        }
    }
}
