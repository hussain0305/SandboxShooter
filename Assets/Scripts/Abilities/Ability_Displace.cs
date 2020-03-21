using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Displace : MonoBehaviour
{
    const int DISPLACE_COST = 50;
    const float DISPLACE_SPEED = 50;
    const int TRAJECTORY_POINTS = 10;

    private bool canDisplace;
    public EPlayerController player;
    public TrailRenderer energyPackTrail;
    public LineRenderer trajectory;

    private Camera cam;
    private Vector3 markerPosition;
    private RaycastHit hit;
    private PhotonView pView;

    void Start()
    {
        cam = Camera.main;
        energyPackTrail.enabled = false;
        trajectory.enabled = false;
        canDisplace = true;
        pView = GetComponent<PhotonView>();
        trajectory.positionCount = TRAJECTORY_POINTS;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine || !canDisplace || player.GetIsUsingOffensive())
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (player.playerEnergy.hasEnergyPack)
            {
                if (player.playerEnergy.GetEnergy() >= DISPLACE_COST)
                {
                    trajectory.enabled = true;
                }
                else
                {
                    player.playerUI.DisplayAlertMessage("You don't have enough energy to blink");
                }
            }
            else
            {
                player.playerUI.DisplayAlertMessage("Can't blink without Energy Pack");
            }

        }

        if(Input.GetButton("Fire2") && trajectory.enabled)
        {
            DisplayTrajectory();
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (player.playerEnergy.hasEnergyPack)
            {
                if (player.playerEnergy.GetEnergy() >= DISPLACE_COST)
                {
                    trajectory.enabled = false;
                }
                else
                {
                    player.playerUI.DisplayAlertMessage("You don't have enough energy to blink");
                }
            }
            else
            {
                player.playerUI.DisplayAlertMessage("Can't blink without Energy Pack");
            }

        }

    }

    void DisplayTrajectory()
    {
        for (int loop = 0; loop < TRAJECTORY_POINTS; loop++)
        {
            trajectory.SetPosition(loop, (cam.transform.position + cam.transform.forward * (loop + 5)));
        }
    }
}
