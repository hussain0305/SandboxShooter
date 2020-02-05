using System.Collections;
using Photon.Pun;
using UnityEngine;

public class CollectibleOrb : MonoBehaviour
{
    public ParticleSystem pickedEffect;

    private PhotonView pView;

    public CollectOrbs parentMode;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
    }

    public void SetLifetime(float lifetime)
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        StartCoroutine(DieIn(lifetime));
        StartCoroutine(IsAboutToDie((4.0f / 5) * lifetime));
    }

    IEnumerator DieIn(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator IsAboutToDie(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<PhotonView>().RPC("RPC_UpdateMaterialOnClients", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UpdateMaterialOnClients()
    {
        GetComponent<Animator>().SetTrigger("AboutToDie");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }
        if (other.GetComponent<EPlayerController>())
        {
            parentMode.OrbPicked(other.GetComponent<EPlayerController>().GetNetworkID());
            pView.RPC("RPC_WasPickedUp", RpcTarget.All);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void RPC_WasPickedUp()
    {
        Instantiate(pickedEffect, transform.position, Quaternion.identity);       
    }
}