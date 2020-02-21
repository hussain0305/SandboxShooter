using Photon.Pun;
using UnityEngine;

public class FlyingGridPlayerDetector : MonoBehaviour
{
    private Rigidbody rBody;
    private PhotonView pView;

    private bool marked = false;

    private void Start()
    {
        rBody = GetComponentInParent<Rigidbody>();
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
            other.GetComponent<PlayerExternalMovement>().AddToVelocity(rBody.velocity);
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
        if (other.GetComponent<EPlayerMovement>())
        {
            other.GetComponent<EPlayerMovement>().SetExternalMovement(false);
        }

    }


}


//public void OnCollisionStay(Collision collision)
//{
//    if (collision.transform.GetComponent<EPlayerController>())
//    {
//        collision.transform.GetComponent<PlayerExternalMovement>().AddToVelocity(rBody.velocity * Time.deltaTime);
//    }
//}


//public void OnCollisionEnter(Collision collision)
//{
//    if (collision.transform.GetComponent<EPlayerMovement>())
//    {
//        collision.transform.GetComponent<EPlayerMovement>().SetExternalMovement(true);
//    }
//}

//public void OnCollisionExit(Collision collision)
//{
//    if (collision.transform.GetComponent<EPlayerMovement>())
//    {
//        collision.transform.GetComponent<EPlayerMovement>().SetExternalMovement(false);
//    }

//}
