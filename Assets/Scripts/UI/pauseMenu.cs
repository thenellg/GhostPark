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
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.Q;
    public KeyCode hold = KeyCode.E;

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
