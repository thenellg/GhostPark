using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public dialogueInterface dialogue;
    public pauseMenu pause;

    private void Start()
    {
        dialogue.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
    }
}
