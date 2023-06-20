using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSettings : MonoBehaviour
{
    [Header("General")]
    public int activeLevel;

    [Header("Unlocks")]
    public bool downSmashUnlock = false;
    public bool dashUnlock = true;

    [Header("Accessibility")]
    public bool infiniteDash;
    public bool invincibility;

    // Start is called before the first frame update
    void Start()
    {

    }
}
