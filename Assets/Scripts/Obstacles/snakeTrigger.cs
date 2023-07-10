using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeTrigger : MonoBehaviour
{
    public GameObject snake;
    public float interval = 0.5f;
    public bool active = false;

    public void setSnake()
    {
        snake.SetActive(true);
        active = true;
        Invoke("disableSnake", 3f);
    }

    public void disableSnake()
    {
        snake.SetActive(false);
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !active)
            Invoke("setSnake", interval);
    }
}
