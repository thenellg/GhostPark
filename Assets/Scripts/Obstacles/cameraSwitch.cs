using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraSwitch : MonoBehaviour
{
    public groupLoading m_groupLoading;
    public Vector3 enterDirection;

    public bool multiRoom = false;
    public List<int> extraRooms;

    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;

    public int group1;
    public int group2;

    public bool forceCheckpointGroup1 = false;
    public bool forceCheckpointGroup2 = false;
    public bool onlyCams = false;

    public bool vertical = false;
    public bool collision = false;

    public void Start()
    {
        m_groupLoading = FindObjectOfType<groupLoading>();
    }

    public void camSwap()
    {
        Invoke("go", 0.3f);

        if (vcam1.Priority > vcam2.Priority)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;

            if (multiRoom)
                m_groupLoading.swapGroups(group2, extraRooms);
            else
                m_groupLoading.swapGroups(group2);
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;

            if (multiRoom)
                m_groupLoading.swapGroups(group1, extraRooms);
            else
                m_groupLoading.swapGroups(group1);
        }

        if (!onlyCams)
        {
            if (forceCheckpointGroup1)
            {
                m_groupLoading.forceCheckpoint = true;
                m_groupLoading.forceGroupID = group1;
                m_groupLoading.forceGroupCam = vcam1;
                m_groupLoading.extraRooms = extraRooms;
            }
            else if (forceCheckpointGroup2)
            {
                m_groupLoading.forceCheckpoint = true;
                m_groupLoading.forceGroupID = group2;
                m_groupLoading.forceGroupCam = vcam2;
                m_groupLoading.extraRooms = extraRooms;
            }
            else
            {
                m_groupLoading.forceCheckpoint = false;
            }
        }
    }

    private void go()
    {
        GameObject.FindObjectOfType<PlayerController>().itemReset();
    }

    public void setEnterDirection(Transform player)
    {
        enterDirection = player.position - transform.position;

        if (collision)
            enterDirection = player.GetComponent<CharacterController2D>().dashVector;
    }

    public void checkCamSwap(Transform player, bool dashCheck = false)
    {
        Vector3 direction = player.position - transform.position;
        if (collision)
            direction = player.GetComponent<Rigidbody2D>().velocity;

        //Debug.Log("Enter: " + enterDirection);
        //Debug.Log("Exit: " + direction);

        if(collision && dashCheck)
        {
            if (vcam1.Priority < vcam2.Priority)
            {
                vcam1.Priority = 1;
                vcam2.Priority = 0;

                if (multiRoom)
                    m_groupLoading.swapGroups(group1, extraRooms);
                else
                    m_groupLoading.swapGroups(group1);
            }
        }
        else if (collision)
        {
            if (vertical && direction.y > 0 && enterDirection.y > 0)
                camSwap();
            else if (vertical && direction.y < 0 && enterDirection.y < 0)
                camSwap();
            else if (!vertical && direction.x > 0 && enterDirection.x > 0)
                camSwap();
            else if (!vertical && direction.x < 0 && enterDirection.x < 0)
                camSwap();
        }
        else
        {
            if (vertical && direction.y > 0 && enterDirection.y < 0)
                camSwap();
            else if (vertical && direction.y < 0 && enterDirection.y > 0)
                camSwap();
            else if (!vertical && direction.x > 0 && enterDirection.x < 0)
                camSwap();
            else if (!vertical && direction.x < 0 && enterDirection.x > 0)
                camSwap();
        }
    }

}
