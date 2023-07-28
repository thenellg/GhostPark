using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class settingsButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void changeText()
    {
        if (text.text == "On")
            text.text = "Off";
        else
            text.text = "On";
    }
}
