using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearingWall : MonoBehaviour
{
    // Start is called before the first frame update
    public float alpha = 1;
    public SpriteRenderer wallImage;
    public bool hidden = false;

    // Update is called once per frame
    void Update()
    {
        if(hidden && alpha > 0)
        {
            alpha -= 0.02f;
            wallImage.color = new Color(wallImage.color.r, wallImage.color.g, wallImage.color.b, alpha);
        }
        else if(!hidden && alpha < 1)
        {
            alpha += 0.02f;
            wallImage.color = new Color(wallImage.color.r, wallImage.color.g, wallImage.color.b, alpha);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hidden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hidden = false;
        }
    }
}
