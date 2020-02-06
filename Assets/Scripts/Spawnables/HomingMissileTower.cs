using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HomingMissileTower : DefensiveBase
{
    public float homingSpeed;
    public HomingNozzle nozzleRotator;
    public Transform nozzle;

    private EPlayerController playerToAttack;

    private void Start()
    {
        if (pView.IsMine)
        {
            StartCoroutine(CheckForEnemies());
        }
    }

    IEnumerator CheckForEnemies()
    {
        while (true)
        {
            playerToAttack = null;
            Collider[] everyoneInRange = Physics.OverlapSphere(transform.position, 40, 1 << 16);
            if (everyoneInRange.Length > 0)
            {
                foreach(Collider currCol in everyoneInRange)
                {
                    if(currCol.GetComponent<EPlayerController>().GetNetworkID() != GetOwner().GetNetworkID())
                    {
                        playerToAttack = currCol.GetComponent<EPlayerController>();
                        break;
                    }
                }
                if (playerToAttack)
                {
                    PerformAttack();
                }
            }
            yield return new WaitForSeconds(1.0f / frequency);
        }
    }

    public override void PerformAttack()
    {
        if (!nozzleRotator.enabled)
        {
            nozzleRotator.enabled = true;
            nozzleRotator.target = playerToAttack.transform;
            nozzleRotator.shouldRotate = true;
        }
        pView.RPC("RPC_SpawnHoming", RpcTarget.All, owner.GetNetworkID(), playerToAttack.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void RPC_SpawnHoming(int id, int victimID)
    {
        Projectile h = Instantiate(projectile, nozzle.position, nozzle.rotation);

        h.transform.LookAt(PhotonView.Find(victimID).transform.position);
        h.SetOwnerID(id);
        h.GetComponent<Rigidbody>().velocity = h.transform.forward * homingSpeed;
    }

}
