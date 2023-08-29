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
    public bool active = false;

    public Transform characterSpot;
    public bool playerFacingRight = false;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera dialogueCam;

    private playerSettings settings;
       

    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.gameObject.SetActive(false);
        settings = FindObjectOfType<playerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
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

        //Dialogue stuff
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
        active = false;

        //change player
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = false;
        player.transform.position = characterSpot.position;

        if (playerFacingRight)
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
        else
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);

        //change camera
        mainCam = FindObjectOfType<Camera>().GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        mainCam.Priority = 0;
        dialogueCam.Priority = 1;
    }

    void endInteract()
    {
        mainCam.Priority = 1;
        dialogueCam.Priority = 0;

        FindObjectOfType<PlayerController>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interactPrompt.gameObject.SetActive(true);
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }
}
