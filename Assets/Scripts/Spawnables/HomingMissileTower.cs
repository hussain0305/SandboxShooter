﻿using Photon.Pun;
using UnityEngine;

public class HomingMissileTower : DefensiveBase
{
    public HomingNozzle nozzleRotator;
    public Transform nozzle;
    public override void PerformAttack()
    {
        if (opponentsInVicinity.Count < 1)
        {
            return;
        }
        if (!nozzleRotator.enabled)
        {
            nozzleRotator.enabled = true;
            nozzleRotator.target = opponentsInVicinity[0].transform;
            nozzleRotator.shouldRotate = true;
        }
        pView.RPC("RPC_SpawnHoming", RpcTarget.All, owner.GetNetworkID());
        StartCoroutine(AttackAgainAfter(frequency));
    }

    public override void StopAttack()
    {
        if (opponentsInVicinity.Count > 0)
        {
            if (nozzleRotator.enabled)
            {
                nozzleRotator.target = null;
                nozzleRotator.shouldRotate = false;
                nozzleRotator.enabled = false;
            }
        }
    }

    [PunRPC]
    void RPC_SpawnHoming(int id)
    {
        Projectile h = Instantiate(projectile, nozzle.position, nozzle.rotation);
        h.GetComponent<HomingMissile>().SetTarget(opponentsInVicinity[0]);
        h.SetOwnerID(id);
    }

}
