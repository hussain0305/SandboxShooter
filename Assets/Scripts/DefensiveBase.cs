using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBase : MonoBehaviour
{
    public Projectile projectile;
    public bool hasProximityDetector;

    public float frequency;
    protected EPlayerController owner;
    protected PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }
    
    public EPlayerController GetOwner()
    {
        return owner;
    }

    public void SetOwner(EPlayerController own)
    {
        if (pView.IsMine)
        {
            pView.RPC("RPC_SetOwner", RpcTarget.All, own.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    public void RPC_SetOwner(int own)
    {
        owner = PhotonView.Find(own).GetComponent<EPlayerController>();
        if (hasProximityDetector)
        {
            GetComponentInChildren<ProximityDetector>().SetOwner(owner);
            GetComponentInChildren<ProximityDetector>().SetStructure(this);
        }
    }

    public virtual void PerformAttack() { }

    public virtual void StopAttack() { }
}
