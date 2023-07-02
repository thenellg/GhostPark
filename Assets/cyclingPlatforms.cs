using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cyclingPlatforms : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public float interval;
    public float timeLeft;
    public int platformNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        updatePlatforms();
        timeLeft = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            updatePlatforms();
            timeLeft = interval;
        }
    }

    void updatePlatforms() { 
        for(int i = 0; i < platforms.Count; i++)
        {
            if (i == platformNum)
                platforms[i].SetActive(true);
            else
                platforms[i].SetActive(false);
        }

        platformNum++;
        if (platformNum >= platforms.Count)
            platformNum = 0;
    }
}
