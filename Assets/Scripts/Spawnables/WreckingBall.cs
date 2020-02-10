using Photon.Pun;
using System.Collections;
using UnityEngine;

public class WreckingBall : MonoBehaviour, IPunObservable
{
    const int damage = 5000;
    const float strength = 250;
    private int pusherID;

    private Vector3 touchDir;
    private PhotonView pView;
    private Rigidbody rBody;
    private PhotonTransformViewClassic pTransformView;

    private void Start()
    {

        pusherID = 0;
        pView = GetComponent<PhotonView>();
        rBody = GetComponent<Rigidbody>();
        pTransformView = GetComponent<PhotonTransformViewClassic>();

        if (!pView.IsMine)
        {
            rBody.useGravity = false;
            rBody.isKinematic = true;
            return;
        }
        //StartCoroutine(SyncValues());

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (other.transform.GetComponent<EPlayerController>())
        {
            pusherID = other.transform.GetComponent<EPlayerController>().GetNetworkID();

            touchDir = other.transform.position - transform.position;
            touchDir.Normalize();

            pView.RPC("RPC_BallHit", RpcTarget.All, touchDir);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pView.IsMine)
        {
            return;
        }
        if (collision.gameObject.GetComponentInParent<SpawnableHealth>())
        {
            collision.gameObject.GetComponentInParent<SpawnableHealth>().TakeDamage(damage, pusherID);
        }

    }

    [PunRPC]
    void RPC_BallHit(Vector3 pTouchDir)
    {
        if (pView.IsMine)
        {
            rBody.AddForce(pTouchDir * strength, ForceMode.Impulse);
        }
    }

    public int GetPusherID()
    {
        return pusherID;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
        throw new System.NotImplementedException();
    }
}
