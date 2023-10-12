using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitch : MonoBehaviour
{
    public string sceneName;

    public bool onEnter = true;
    public bool active = false;
    public playerSettings settings;

    public GameObject showingImage;

    public bool ifEndLevel = false;
    public bool overrideStartSpot = false;
    public int overrideGroup;
    public string overrideCameraName;
    public List<int> overrideExtraRooms;

    private void Start()
    {
        settings = FindObjectOfType<playerSettings>();
        if(showingImage && !ifEndLevel)
            showingImage.SetActive(false);
    }


    private void Update()
    {
        if(active && (Input.GetKeyDown(settings.jump) || Input.GetKeyDown(settings.hold)))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void setLoadOverride()
    {
        settings.overrideLoad = true;
        settings.overrideGroup = overrideGroup;
        settings.overrideCameraName = overrideCameraName;
        settings.overrideExtraRooms = overrideExtraRooms;
    }

    public void loadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !ifEndLevel)
        {
            if (onEnter)
            {
                if (overrideStartSpot)
                    setLoadOverride();

                loadScene();
            }
            else
            {
                active = true;
                showingImage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !ifEndLevel)
        {
            active = false;
            showingImage.SetActive(false);
        }
    }

}
