using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool newGame = true;
    public mainMenu menu;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (newGame)
                menu.newGame();
            else
                menu.loadGame();
        }
    }
}
