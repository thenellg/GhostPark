using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;

    public groupLoading group1;
    public groupLoading group2;

    public void camSwap()
    {
        Invoke("go", 0.3f);

        if (vcam1.Priority > vcam2.Priority)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
            group2.swapGroups();
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
            group1.swapGroups();
        }
    }

    private void go()
    {
        GameObject.FindObjectOfType<PlayerController>().itemReset();
    }
}
