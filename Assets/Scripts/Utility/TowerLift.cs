using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TowerLift : MonoBehaviour
{
    private PhotonView pView;
    private Animator animate;

    void Start()
    {
        pView = GetComponent<PhotonView>();
        animate = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<EPlayerController>() || !other.GetComponent<EPlayerController>().IsLocalPView())
        {
            return;
        }

        pView.RPC("RPC_LiftMotion", RpcTarget.All, true);// upPosition);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<EPlayerController>() || !other.GetComponent<EPlayerController>().IsLocalPView())
        {
            return;
        }
        pView.RPC("RPC_LiftMotion", RpcTarget.All, false);
    }

    [PunRPC]
    void RPC_LiftMotion(bool goUp)
    {
        if (goUp)
        {
            animate.SetTrigger("GoUp");
        }
        else
        {
            animate.SetTrigger("GoDown");
        }
    }
}