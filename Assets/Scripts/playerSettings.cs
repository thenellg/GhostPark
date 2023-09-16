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
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.Q;
    public KeyCode hold = KeyCode.E;

    public List<KeyCode> keyboardControls = new List<KeyCode>();
    public List<KeyCode> playstationControls = new List<KeyCode>();
    public List<KeyCode> xboxControls = new List<KeyCode>();
    public List<KeyCode> switchControls = new List<KeyCode>();

    public bool active = false;

    private void Start()
    {
        resetDefault();
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
            checkController();
    }

    public void checkController()
    {
        var controllers = Input.GetJoystickNames();
        //foreach (string controller in controllers)
            //Debug.Log(controller);
        active = false;

        if (controllers.Length > 0 && controllers[0].Length > 0)
        {
            if (controllers[0].ToLower().Contains("dualsense") || controllers[0].ToLower().Contains("playstation"))
                currentController = controlType.Playstation;
            else if (controllers[0].ToLower().Contains("pro controller"))
                currentController = controlType.Switch;
            else if (controllers[0].ToLower().Contains("xbox"))
                currentController = controlType.Xbox;
            else
                currentController = controlType.keyboard;
        }
        else
        {
            currentController = controlType.keyboard;
        }
        resetDefault();
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
            keyboardControls[0] = KeyCode.W;
            keyboardControls[1] = KeyCode.S;
            keyboardControls[2] = KeyCode.A;
            keyboardControls[3] = KeyCode.D;
            keyboardControls[4] = KeyCode.Space;
            keyboardControls[5] = KeyCode.Q;
            keyboardControls[6] = KeyCode.E;
        }
        if (currentController == controlType.Playstation)
        {
            playstationControls[0] = KeyCode.W;
            playstationControls[1] = KeyCode.S;
            playstationControls[2] = KeyCode.A;
            playstationControls[3] = KeyCode.D;
            playstationControls[4] = KeyCode.JoystickButton1;
            playstationControls[5] = KeyCode.JoystickButton0;
            playstationControls[6] = KeyCode.JoystickButton2;
        }
        else if (currentController == controlType.Xbox)
        {
            xboxControls[0] = KeyCode.W;
            xboxControls[1] = KeyCode.S;
            xboxControls[2] = KeyCode.A;
            xboxControls[3] = KeyCode.D;
            xboxControls[4] = KeyCode.JoystickButton0;
            xboxControls[5] = KeyCode.JoystickButton2;
            xboxControls[6] = KeyCode.JoystickButton1;
        }
        else if (currentController == controlType.Switch)
        {
            switchControls[0] = KeyCode.W;
            switchControls[1] = KeyCode.S;
            switchControls[2] = KeyCode.A;
            switchControls[3] = KeyCode.D;
            switchControls[4] = KeyCode.JoystickButton0;
            switchControls[5] = KeyCode.JoystickButton2;
            switchControls[6] = KeyCode.JoystickButton1;
        }
        changeSettings();
    }
}
