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

    [Header("Accessibility")]
    public bool infiniteDash;
    public bool invincibility;
    public bool outline;

    [Header("Key Bindings")]
    public controlType currentController = controlType.keyboard;
    public string up = "w";
    public string down = "s";
    public string left = "a";
    public string right = "d";
    public string jump = "space";
    public string dash = "q";
    public string hold = "e";

    public List<string> keyboardDefault = new List<string>();
    public List<string> playstationDefault = new List<string>();
    public List<string> xboxDefault = new List<string>();
    public List<string> switchDefault = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

    }
}
