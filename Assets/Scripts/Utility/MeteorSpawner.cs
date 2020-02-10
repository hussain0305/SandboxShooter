using Photon.Pun;
using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    const float RESTART_TIMER = 15.0f;
    public Transform spawnPositions;
    public ParticleSystem activatedEffect;
    public Meteor meteor;

    private int activatorID = 0;
    private Renderer render;
    private SphereCollider col;
    private PhotonView pView;

    private Meteor spawnedMeteor;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        render = GetComponent<MeshRenderer>();
        col = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pView.IsMine)
        {
            return;
        }

        if (other.transform.GetComponent<EPlayerController>())
        {
            pView.RPC("RPC_MeteorShowerCalled", RpcTarget.All, other.transform.GetComponent<EPlayerController>().GetNetworkID());
        }
    }

    [PunRPC]
    void RPC_MeteorShowerCalled(int id)
    {
        activatorID = id;
        render.enabled = false;
        col.enabled = false;
        StartCoroutine(ResetMeteorSpawner());
        Instantiate(activatedEffect, transform.position, transform.rotation);
        SpawnMeteors();
    }


    IEnumerator ResetMeteorSpawner()
    {
        yield return new WaitForSeconds(RESTART_TIMER);
        render.enabled = true;
        col.enabled = true;
    }

    public void SpawnMeteors()
    {
        foreach(Transform currSpawn in spawnPositions)
        {
            spawnedMeteor = Instantiate(meteor, currSpawn.position, currSpawn.rotation).GetComponent<Meteor>();
            spawnedMeteor.SetSummonerID(activatorID);
        }
    }
}
