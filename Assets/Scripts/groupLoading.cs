using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groupLoading : MonoBehaviour
{
    public GameObject groupContainer;
    public List<groupLoading> connectedGroups = new List<groupLoading>();

    public void swapGroups()
    {
        foreach(groupLoading group in groupContainer.GetComponentsInChildren<groupLoading>())
        {
            if(connectedGroups.IndexOf(group) >= 0)
                group.gameObject.SetActive(true);
            else
                group.gameObject.SetActive(false);
        }
    }
}
