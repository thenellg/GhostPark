using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groupLoading : MonoBehaviour
{
    public List<GameObject> groups = new List<GameObject>();

    private void Start()
    {
        for (int i = 2; i < groups.Count; i++)
        {
            groups[i].SetActive(false);
        }
    }

    public void swapGroups(int mainID, List<int> extraRooms = null)
    {
        for(int i = 0; i < groups.Count; i++)
        {
            if (i <= mainID + 1 && i >= mainID - 1)
                groups[i].SetActive(true);
            else
                groups[i].SetActive(false);
        }

        if(extraRooms != null)
        {
            foreach(int roomNum in extraRooms)
            {
                groups[roomNum].SetActive(true);
            }
        }
    }
}
