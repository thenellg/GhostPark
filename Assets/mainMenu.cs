using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject loadgametext;
    public GameObject newgametext;
    public GameObject pressanybuttontext;

    public CinemachineVirtualCamera start;
    public CinemachineVirtualCamera end;

    private void Start()
    {
        loadgametext.SetActive(false);
        newgametext.SetActive(false);
        pressanybuttontext.SetActive(true);
    }

    void transitionMenu()
    {
        start.Priority = 0;
        end.Priority = 1;

        loadgametext.SetActive(true);
        newgametext.SetActive(true);
        pressanybuttontext.SetActive(false);
    }

    public void loadGame()
    {
        //Will actually make one here soon but for now
        newGame();
    }

    public void newGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void Update()
    {
        if (pressanybuttontext.activeSelf)
            if (Input.anyKeyDown)
                transitionMenu();
    }
}
