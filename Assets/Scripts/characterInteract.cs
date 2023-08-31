using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class characterInteract : MonoBehaviour
{
    public enum CharacterType { Dialogue, Store, Gate }
    public CharacterType characterType;

    public SpriteRenderer interactPrompt;
    public Sprite interactionArt;
    public bool triggerActive = false;
    public bool dialogueActive = false;

    public Transform characterSpot;
    public bool playerFacingRight = false;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera dialogueCam;

    private playerSettings settings;

    public dialogueInterface dialogue;
    public dialogue startMessage;

    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.gameObject.SetActive(false);
        dialogue = FindObjectOfType<UIHandler>().dialogue;
        settings = FindObjectOfType<playerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerActive && !dialogueActive)
        {
            if (Input.GetKeyDown(settings.hold)) 
            {
                if(characterType == CharacterType.Dialogue)
                    startDialogue();
                else if (characterType == CharacterType.Store)
                    startStore();
                else if (characterType == CharacterType.Gate)
                    startGate();
            }
        }
    }

    void startDialogue()
    {
        startInteract();
        dialogue.dialogue = startMessage;
        dialogue.currentChar = this;

        dialogue.setOptions(startMessage);
        dialogue.setBackground(startMessage.backgroundImage, Color.white);
        dialogue.setText(startMessage.speakerName, startMessage.speakerMessage);

        //Dialogue stuff
        dialogueActive = true;
        dialogue.gameObject.SetActive(true);
    }
    void startStore()
    {
        startInteract();

        //Dialogue stuff
    }
    void startGate()
    {

    }

    void startInteract()
    {
        //change player
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = false;
        player.controller.m_Rigidbody2D.velocity = Vector2.zero;
        player.transform.position = characterSpot.position;
        player.controller.PlayerAnim.SetBool("isWalking", false);

        if (playerFacingRight)
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
        else
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);

        //change camera
        mainCam = FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        mainCam.Priority = 0;
        dialogueCam.Priority = 1;
    }

    public void endInteract()
    {
        dialogue.gameObject.SetActive(false);
        dialogue.currentChar = null;

        dialogueActive = false;

        mainCam.Priority = 1;
        dialogueCam.Priority = 0;

        FindObjectOfType<PlayerController>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interactPrompt.gameObject.SetActive(true);
            triggerActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactPrompt.gameObject.SetActive(false);
            triggerActive = false;
        }
    }
}
