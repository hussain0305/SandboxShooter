using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject shield;

    private Vector3 shieldShrunkScale;
    private Vector3 shieldOriginalScale;

    private PhotonView pView;

    // Start is called before the first frame update
    void Awake()
    {
        shieldOriginalScale = shield.transform.localScale;
        shieldShrunkScale = new Vector3(shieldOriginalScale.x, shieldOriginalScale.y / 4, shieldOriginalScale.z);
        pView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!pView || !pView.IsMine)
        {
            return;
        }

        if (other.transform.gameObject.GetComponent<EPlayerController>())
        {
            pView.RPC("RPC_Shrink", RpcTarget.All);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pView || !pView.IsMine)
        {
            return;
        }

        if (other.transform.gameObject.GetComponent<EPlayerController>())
        {
            pView.RPC("RPC_Expand", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_Shrink()
    {
        StopAllCoroutines();
        if (shield)
        {
            StartCoroutine(ShrinkShield());
        }
    }

    [PunRPC]
    void RPC_Expand()
    {
        StopAllCoroutines();
        if (shield)
        {
            StartCoroutine(ExpandShield());
        }
    }


    IEnumerator ShrinkShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldShrunkScale) > 0.1f)
        {
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldShrunkScale, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        shield.transform.localScale = shieldShrunkScale;
    }

    IEnumerator ExpandShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldOriginalScale) > 0.1f)
        {
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldOriginalScale, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        shield.transform.localScale = shieldOriginalScale;
    }
}
