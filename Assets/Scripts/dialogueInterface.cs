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
}
