using Photon.Pun;
using UnityEngine;

public class FlyingGridPlayerDetector : MonoBehaviour
{
    private Rigidbody rBody;
    private AutoNavFlyer flyer;
    private PhotonView pView;

    private bool marked = false;

    private void Start()
    {
        rBody = GetComponentInParent<Rigidbody>();
        //flyer = GetComponentInParent<AutoNavFlyer>();
        pView = GetComponentInParent<PhotonView>();
    }


    public void OnTriggerStay(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (other.GetComponent<EPlayerController>())
        {
            other.GetComponent<PlayerExternalMovement>().SetVelocity(rBody.velocity);//flyer.GetDirectionalVelocity()
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (!marked)
        {
            GetComponentInParent<AutoNavFlyer>().StartMoving();
            marked = true;
        }

        if (other.GetComponent<EPlayerMovement>())
        {
            other.GetComponent<EPlayerMovement>().SetExternalMovement(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (other.GetComponent<EPlayerController>())
        {
            other.GetComponent<PlayerExternalMovement>().SetVelocity(Vector3.zero);
        }

        if (other.GetComponent<EPlayerMovement>())
        {
            other.GetComponent<EPlayerMovement>().SetExternalMovement(false);
        }

    }


}