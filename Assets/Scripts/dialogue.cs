using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class dialogue : ScriptableObject
{
    public string speakerName;
    public string speakerMessage;
    public Sprite backgroundImage;

    public List<string> options = new List<string>();
    public List<dialogue> nextMessage = new List<dialogue>();
}
