using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class forceDialogueTrigger : MonoBehaviour
{
    public Transform characterSpot;
    public bool playerFacingRight = false;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera dialogueCam;

    private playerSettings settings;

    public dialogueInterface dialogue;
    public dialogue startMessage;
    public bool active = false;
    public bool endLevel = false;
    public string hubLevelName;

    public enum abilities
    {
        none,
        dash,
        glide
    }
    public abilities unlock;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = FindObjectOfType<UIHandler>().dialogue;
        settings = FindObjectOfType<playerSettings>();
    }

    public void startDialogue()
    {
        active = true;

        //change player
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = false;
        player.controller.m_Rigidbody2D.velocity = Vector2.zero;
        player.controller.PlayerAnim.SetBool("isWalking", false);
        player.controller.PlayerAnim.SetBool("isGrounded", true);

        if (playerFacingRight)
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
        else
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
        player.transform.position = characterSpot.position;

        //change camera
        mainCam = FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        mainCam.Priority = 0;
        dialogueCam.Priority = 1;

        //Dialogue
        dialogue.dialogue = startMessage;
        dialogue.forced = true;
        dialogue.forcedTrigger = this;
        //dialogue.currentChar = this;

        dialogue.setOptions(startMessage);
        dialogue.setBackground(startMessage.backgroundImage, Color.white);
        dialogue.setText(startMessage.speakerName, startMessage.speakerMessage);

        //Dialogue stuff
        dialogue.gameObject.SetActive(true);
    }

    public void endInteract()
    {
        dialogue.gameObject.SetActive(false);
        dialogue.forcedTrigger = null;
        dialogue.forced = false;


        mainCam.Priority = 1;
        dialogueCam.Priority = 0;

        if (endLevel)
        {
            GetComponent<sceneSwitch>().loadScene();
        }

        FindObjectOfType<PlayerController>().canMove = true;
        Invoke("byebye", 1f);
    }

    void byebye()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!active && collision.tag == "Player")
        {
            startDialogue();

            if(unlock != abilities.none)
            {
                if (unlock == abilities.dash)
                    settings.dashUnlock = true;
                else if (unlock == abilities.glide)
                    settings.glideUnlock = true;
            }
        }
    }
}
