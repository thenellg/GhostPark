using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileShoot : MonoBehaviour
{
    public BoxCollider2D triggerArea;
    public List<GameObject> projectiles = new List<GameObject>();
    public Transform originPoint;
    public bool consistent = false;

    public float interval;
    public float timer;
    public bool directionLeft = true;
    public bool active = false;

    public bool possessable = false;
    private int count = 0;
    public int possessionInterval;

    public enum projectileType
    {
        fireball,
        fallingRockFatal,
        fallingRockObstacle
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
    private void Update()
    {
        if (active)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
                instantiateProjectile();
        }    
    }

    public void instantiateProjectile()
    {
        GameObject _projectile = null;

        if (m_projectileType == projectileType.fireball)
        {
            _projectile = Instantiate(projectiles[0]);

            if (directionLeft)
                _projectile.GetComponent<fireball>().speed *= -1;

            //Set necessities for fireball
        }
        else if (m_projectileType == projectileType.fallingRockFatal)
        {
            _projectile = Instantiate(projectiles[1]);
            _projectile.GetComponent<fallingRock>().fatal = true;
        }
        else if (m_projectileType == projectileType.fallingRockObstacle)
        {
            _projectile = Instantiate(projectiles[1]);
            _projectile.GetComponent<fallingRock>().fatal = false;
        }

        _projectile.transform.position = originPoint.position;

        if (possessable)
        {
            count++;
            if(count == possessionInterval)
            {
                _projectile.AddComponent<possessionObject>();
                count = 0;
                _projectile.GetComponent<SpriteRenderer>().color = Color.green;
            }

        }

        timer = interval;
    }

    void resetInterval()
    {
        if (active)
            instantiateProjectile();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player" || collision.tag == "Minecart") && !consistent)
        {
            timer = interval;
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if((collision.tag == "Player" || collision.tag == "Minecart") && !consistent )
        {
            active = false;
        }
    }
}
