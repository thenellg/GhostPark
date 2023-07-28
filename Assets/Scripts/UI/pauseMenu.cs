using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pauseMenu : MonoBehaviour
{

    [Header("General")]
    public GameObject mainPauseMenu;
    public GameObject settingsMenu;

    [Header("Settings")]
    public playerSettings settings;

    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float menuVolume = 1f;

    public bool infiniteDash = false;
    public bool invincibility = false;
    public bool coverPits = false;

    [Header("Key Bindings")]
    public string up = "w";
    public string down = "s";
    public string left = "a";
    public string right = "d";
    public string jump = "space";
    public string dash = "q";
    public string hold = "e";

    public void Start()
    {
        settings = FindObjectOfType<playerSettings>();
        setTempSettings();
    }
    
    public void turnOffPauseMenu()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void returnToMainMenu()
    {
        
    }

    public void setTempSettings()
    {
        masterVolume = settings.masterVolume;
        sfxVolume = settings.sfxVolume;
        musicVolume = settings.musicVolume;
        menuVolume = settings.menuVolume;

        infiniteDash = settings.infiniteDash;
        invincibility = settings.invincibility;
        coverPits = settings.coverPits;

        up = settings.up;
        down = settings.down;
        left = settings.left;
        right = settings.right;
        jump = settings.jump;
        dash = settings.dash;
        hold = settings.hold;

    }

    public void setActualSettings()
    {
        settings.masterVolume = masterVolume;
        settings.sfxVolume = sfxVolume;
        settings.musicVolume = musicVolume;
        settings.menuVolume = menuVolume;

        settings.infiniteDash = infiniteDash;
        settings.invincibility = invincibility;
        settings.coverPits = coverPits;

        settings.up = up;
        settings.down = down;
        settings.left = left;
        settings.right = right;
        settings.jump = jump;
        settings.dash = dash;
        settings.hold = hold;

        //if (coverPits)
    }

    public void changeDash()
    {
        Debug.Log("check button");
        infiniteDash = !infiniteDash;
    }

    public void changeInvincible()
    {
        Debug.Log("check button");
        invincibility = !invincibility;
    }

    public void changePits()
    {
        Debug.Log("check button");
        coverPits = !coverPits;
        coverPits = true;
    }
}
