using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TowerLift : MonoBehaviour
{
    public Vector3 downPosition;
    public Vector3 upPosition;
    public float liftSpeed;

    private Vector3 destination;

    private PhotonView pView;

    void Start()
    {
        transform.localPosition = downPosition;
        pView = GetComponent<PhotonView>();
    }

    IEnumerator MoveLift()
    {
        GetComponent<Rigidbody>().velocity = transform.up * liftSpeed *
            ((transform.localPosition.y < destination.y) ? 1 : -1);
        while (Vector3.Distance(transform.localPosition, destination) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localPosition = destination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<EPlayerController>() || !other.GetComponent<EPlayerController>().IsLocalPView())
        {
            return;
        }

        pView.RPC("RPC_LiftMotion", RpcTarget.All, upPosition);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<EPlayerController>() || !other.GetComponent<EPlayerController>().IsLocalPView())
        {
            return;
        }
        pView.RPC("RPC_LiftMotion", RpcTarget.All, downPosition);
    }

    [PunRPC]
    void RPC_LiftMotion(Vector3 tDestination)
    {
        destination = tDestination;
        StopAllCoroutines();
        StartCoroutine(MoveLift());

    }
}