using Photon.Pun;
using System.IO;
using UnityEngine;

public class ChainPlatform : MonoBehaviour
{
    public string[] objectToSpawnLocation;
    public float spawnLocationOffset;
    public Color spentMaterial;
    public int platformHealth;
    
    private PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
    }

    public void PlatformTriggered(int index)
    {
        if (!pView.IsMine)
        {
            return;
        }
        Vector3 targetPos = transform.GetChild(index).transform.position + transform.GetChild(index).transform.forward * spawnLocationOffset;
        if(Physics.CheckSphere(targetPos, 8.8f))
        {
            return;
        }
        pView.RPC("RPC_PlatformTriggered", RpcTarget.All, index);
    }

    [PunRPC]
    void RPC_PlatformTriggered(int index)
    {
        SpawnPad triggeredPad = transform.GetChild(index).GetComponent<SpawnPad>();
        GameObject platform = PhotonNetwork.Instantiate(Path.Combine(objectToSpawnLocation), 
            (triggeredPad.transform.position + triggeredPad.transform.forward * spawnLocationOffset),
            triggeredPad.transform.rotation, 0);
        platform.GetComponent<SpawnableHealth>().InitiateSystems(platformHealth);
        triggeredPad.GetComponent<SphereCollider>().enabled = false;
        triggeredPad.GetComponent<Renderer>().material.color = spentMaterial;

    }
}
