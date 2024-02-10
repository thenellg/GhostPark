using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class dialogueInterface : MonoBehaviour
{
    public Image backgroundArt;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    public dialogue dialogue;
    public characterInteract currentChar;
    public forceDialogueTrigger forcedTrigger;

    public playerSettings settings;
    public int selection = 0;

    public Button[] optionButtons;
    public bool forced = false;

    private void Start()
    {
        settings = FindObjectOfType<playerSettings>();
    }
    public void setBackground(Sprite image, Color imageColor)
    {
        backgroundArt.sprite = image;
        backgroundArt.color = imageColor;
    }

    public void setText(string name, string message)
    {
        nameText.text = name;
        messageText.text = message;
    }

    public void setOptions(dialogue dialogue)
    {
        if (dialogue.options.Count > 1)
        {
            GetComponent<RectTransform>().localPosition = new Vector3(-58, -204, 0);

            for (int i = 0; i < 3; i++)
            {
                if (i < dialogue.options.Count)
                {
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogue.options[i];
                    optionButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            GetComponent<RectTransform>().localPosition = new Vector3(0, -204, 0);

            for (int i = 0; i < 3; i++)
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (dialogue)
        {
            if (Input.GetKeyDown(settings.hold) || Input.GetKeyDown(settings.jump) || Input.GetKeyDown(settings.dash))
            {
                if (dialogue.options.Count == 1)
                {
                    //Debug.Log("test");
                    nextDialogue(0);
                }
                else if (dialogue.options.Count < 1)
                {
                    if (!forced)
                        currentChar.endInteract();
                    else
                        forcedTrigger.endInteract();

                    dialogue = null;
                    currentChar = null;
                }
            }
        }
    }

    public void nextDialogue(int choice)
    {
        if (choice < dialogue.nextMessage.Count)
        {
            setOptions(dialogue.nextMessage[choice]);
            setText(dialogue.nextMessage[choice].speakerName, dialogue.nextMessage[choice].speakerMessage);
            dialogue = dialogue.nextMessage[choice];
        }
        else
        {
            Debug.Log("BROKEN");
        }
    }
}
