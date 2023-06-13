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

    [Header("Castle Level")]
    public int castleCollectibles;
    public int castleCollected = 0;
    public int castleCurrentCam;
    public bool castleBeaten = false;

    [Header("Space Level")]
    public int spaceCollectibles;
    public int spaceCollected = 0;
    public int spaceCurrentCam;
    public bool spaceBeaten = false;

    // Start is called before the first frame update
    void Start()
    {

    }
}
