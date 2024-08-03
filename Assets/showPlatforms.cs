using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showPlatforms : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();

    void show()
    {
        foreach(GameObject platform in platforms)
        {
            platform.SetActive(true);
        }
    }

    public void hide()
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
    }

    private void OnEnable()
    {
        hide();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            show();
        }
    }
}
