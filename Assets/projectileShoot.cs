using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileShoot : MonoBehaviour
{
    public BoxCollider2D triggerArea;
    public GameObject projectile;
    public GameObject originPoint;
    public bool consistent = false;

    public int interval;
    public bool directionLeft = true;
    public bool active = false;

    public bool possessable = false;
    private int count = 0;
    public int possessionInterval;

    public enum projectileType
    {
        fireball,
        fallingRock
    }
    public projectileType m_projectileType;

    // Start is called before the first 1frame update
    void Start()
    {
        if (consistent)
        {
            active = true;
            instantiateProjectile();
        }
    }

    public void instantiateProjectile()
    {
        GameObject _projectile = Instantiate(projectile);

        if (m_projectileType == projectileType.fireball)
        {
            if (directionLeft)
                _projectile.GetComponent<fireball>().speed *= -1;
            //Set necessities for fireball
        }
        else if (m_projectileType == projectileType.fallingRock)
        {

        }

        if (possessable)
        {
            count++;
            if(count == possessionInterval)
            {
                projectile.AddComponent<possessionObject>();
                count = 0;
                _projectile.GetComponent<SpriteRenderer>().color = Color.green;
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
        if(collision.tag == "Player" && !consistent)
        {
            active = true;
            Invoke("instantiateProjectile",1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !consistent)
        {
            active = false;
        }
    }
}
