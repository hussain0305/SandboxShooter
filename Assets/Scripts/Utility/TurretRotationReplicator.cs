using Photon.Pun;
using UnityEngine;

public class TurretRotationReplicator : MonoBehaviour, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

}

