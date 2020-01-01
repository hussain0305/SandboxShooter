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
    protected List<EPlayerController> opponentsInVicinity;
    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }
    void Start()
    {
        opponentsInVicinity = new List<EPlayerController>();
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

    public void OpponentDetected(EPlayerController opponent)
    {
        opponentsInVicinity.Add(opponent);
        PerformAttack();
    }

    public void OpponentLeft(EPlayerController opponent)
    {
        if (opponentsInVicinity.Contains(opponent))
        {
            opponentsInVicinity.Remove(opponent);
        }
        StopAttack();
    }

    public IEnumerator AttackAgainAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        PerformAttack();
    }

    public virtual void PerformAttack() { }

    public virtual void StopAttack() { }
}
