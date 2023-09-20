using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groupLoading : MonoBehaviour
{
    public List<GameObject> groups = new List<GameObject>();
    public List<Transform> checkpoints = new List<Transform>();
    public List<int> reverseGravCheckpoints = new List<int>();
    public int activeGroup = 0;

    private void Start()
    {
        swapGroups(activeGroup);
    }

    public Vector3 returnCheckpoint()
    {
        return checkpoints[activeGroup].position;
    }

    public void swapGroups(int mainID, List<int> extraRooms = null)
    {
        for(int i = 0; i < groups.Count; i++)
        {
            if (i <= mainID + 1 && i >= mainID - 1)
                groups[i].SetActive(true);
            else
                groups[i].SetActive(false);

            activeGroup = mainID;
        }

        if(extraRooms != null)
        {
            foreach(int roomNum in extraRooms)
            {
                groups[roomNum].SetActive(true);
            }
        }
    }

    public bool checkGrav()
    {
        return reverseGravCheckpoints.Contains(activeGroup);
    }
}
