using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{

    [Header("General")]
    public GameObject mainPauseMenu;
    public GameObject settingsMenu;
    
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    [Header("Settings")]
    public playerSettings settings;

    public Toggle dashToggle;
    public Toggle invincibilityToggle;
    public Toggle pitsToggle;

    [Header("Audio")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider menuSlider;

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

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        mainPauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    
    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void turnOffPauseMenu()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void toggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void returnToMainMenu()
    {
        
    }

    public void setTempSettings()
    {
        masterSlider.value = settings.masterVolume;
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;
        menuSlider.value = settings.menuVolume;

        dashToggle.isOn = settings.infiniteDash;
        invincibilityToggle.isOn = settings.invincibility;
        pitsToggle.isOn = settings.coverPits;

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
        settings.masterVolume = masterSlider.value;
        settings.sfxVolume = sfxSlider.value;
        settings.musicVolume = musicSlider.value;
        settings.menuVolume = menuSlider.value;

        settings.infiniteDash = dashToggle.isOn;
        settings.invincibility = invincibilityToggle.isOn;
        settings.coverPits = pitsToggle.isOn;

        settings.up = up;
        settings.down = down;
        settings.left = left;
        settings.right = right;
        settings.jump = jump;
        settings.dash = dash;
        settings.hold = hold;

        //if (coverPits)
    }
}
