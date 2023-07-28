using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{

    public GameObject mainPauseMenu;
    public GameObject settingsMenu;



    public void turnOffPauseMenu()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void returnToMainMenu()
    {
        
    }
}
