using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraDollyFollow : MonoBehaviour
{
    public CinemachineDollyCart cart;
    public CinemachinePath path;
    public PlayerController player;
    public float test = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        cart.m_Position = test = path.FindClosestPoint(player.transform.position, 0, -1, 10);
    }
}
