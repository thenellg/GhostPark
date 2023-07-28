using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSettings : MonoBehaviour
{
    public enum controlType { Playstation, Xbox, Switch, keyboard };

    [Header("General")]
    public int activeLevel;

    [Header("Unlocks")]
    public bool downSmashUnlock = false;
    public bool dashUnlock = true;

    [Header("Audio")]
    [Range(0f, 1f)] public float masterVolume;
    [Range(0f, 1f)] public float sfxVolume;
    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float menuVolume;

    [Header("Level Complete")]
    public bool fantasyComplete = false;
    public bool futureComplete = false;
    public bool horrorComplete = false;

    [Header("Accessibility")]
    public bool infiniteDash;
    public bool invincibility;
    public bool coverPits;

    [Header("Key Bindings")]
    public controlType currentController = controlType.keyboard;
    public string up = "w";
    public string down = "s";
    public string left = "a";
    public string right = "d";
    public string jump = "space";
    public string dash = "q";
    public string hold = "e";

    public List<string> keyboardControls = new List<string>();
    public List<string> playstationControls = new List<string>();
    public List<string> xboxControls = new List<string>();
    public List<string> switchControls = new List<string>();

    public bool active = false;

    private void Update()
    {
        if (active)
        {
            var controllers = Input.GetJoystickNames();
            foreach (string controller in controllers)
                Debug.Log(controller);
            active = false;
        }
    }

    public void changeSettings()
    {
        if (currentController == controlType.keyboard)
        {
            up = keyboardControls[0];
            down = keyboardControls[1];
            left = keyboardControls[2];
            right = keyboardControls[3];
            jump = keyboardControls[4];
            dash = keyboardControls[5];
            hold = keyboardControls[6];
}
        if (currentController == controlType.Playstation)
        {
            up = playstationControls[0];
            down = playstationControls[1];
            left = playstationControls[2];
            right = playstationControls[3];
            jump = playstationControls[4];
            dash = playstationControls[5];
            hold = playstationControls[6];
        }
        else if (currentController == controlType.Xbox)
        {
            up = xboxControls[0];
            down = xboxControls[1];
            left = xboxControls[2];
            right = xboxControls[3];
            jump = xboxControls[4];
            dash = xboxControls[5];
            hold = xboxControls[6];
        }
        else if (currentController == controlType.Switch)
        {
            up = switchControls[0];
            down = switchControls[1];
            left = switchControls[2];
            right = switchControls[3];
            jump = switchControls[4];
            dash = switchControls[5];
            hold = switchControls[6];
        }
    }

    public void resetDefault()
    {
        if (currentController == controlType.keyboard)
        {
            keyboardControls[0] = "w";
            keyboardControls[1] = "s";
            keyboardControls[2] = "a";
            keyboardControls[3] = "d";
            keyboardControls[4] = "space";
            keyboardControls[5] = "q";
            keyboardControls[6] = "e";
        }
        if (currentController == controlType.Playstation)
        {
            playstationControls[0] = "w";
            playstationControls[1] = "s";
            playstationControls[2] = "a";
            playstationControls[3] = "d";
            playstationControls[4] = "space";
            playstationControls[5] = "q";
            playstationControls[6] = "e";
        }
        else if (currentController == controlType.Xbox)
        {
            xboxControls[0] = "w";
            xboxControls[1] = "s";
            xboxControls[2] = "a";
            xboxControls[3] = "d";
            xboxControls[4] = "space";
            xboxControls[5] = "q";
            xboxControls[6] = "e";
        }
        else if (currentController == controlType.Switch)
        {
            switchControls[0] = "w";
            switchControls[1] = "s";
            switchControls[2] = "a";
            switchControls[3] = "d";
            switchControls[4] = "space";
            switchControls[5] = "q";
            switchControls[6] = "e";
        }
        changeSettings();
    }
}
