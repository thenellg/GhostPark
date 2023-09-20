using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockAbility : MonoBehaviour
{
    public enum abilities
    {
        dash,
        glide
    }

    public abilities unlock;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerSettings settings = FindObjectOfType<playerSettings>();

            if (unlock == abilities.dash)
                settings.dashUnlock = true;
            else if (unlock == abilities.glide)
                settings.glideUnlock = true;
        }    
    }
}
