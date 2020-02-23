using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SpawnableAnchor : MonoBehaviour
{
    private PhotonView pView;

    private int gridID;
    void Start()
    {
        //See if theres flying grid below
        //if there is, parent yourself to it
        //next step : adjust position


        pView = GetComponent<PhotonView>();
        if (!pView.IsMine)
        {
            return;
        }

        if (HasFlyingGridBelow(out gridID))
        {
            pView.RPC("RPC_AttachYourselfTo", RpcTarget.All, gridID);
        }

        if (!GetComponent<AutoNavFlyer>())
        {
            StartCoroutine(HasSomethingBelow());
        }
    }

    bool HasFlyingGridBelow(out int gridID)
    {
        Collider[] allHits = Physics.OverlapSphere(transform.position, 1, (1 << LayerMask.NameToLayer("FloatingPlatform")));
        foreach(Collider currCol in allHits)
        {
            if (currCol.transform.GetComponent<FlyingGridChoppers>())
            {
                gridID = currCol.transform.GetComponentInParent<PhotonView>().ViewID;
                return true;
            }
        }
        gridID = 0;
        return false;
    }

    IEnumerator HasSomethingBelow()
    {
        Collider[] allHits;
        int count = 0;
        while (count < 5)
        {
            allHits = Physics.OverlapSphere(transform.position, 1, (1 << LayerMask.NameToLayer("Bounds") | 1 << LayerMask.NameToLayer("FloatingPlatform")));
            if (allHits.Length == 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }

            count++;
            yield return new WaitForSeconds(2);
        }
    }


    [PunRPC]
    void RPC_AttachYourselfTo(int ID)
    {
        transform.parent = PhotonView.Find(ID).transform;
    }

}
