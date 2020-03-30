using Photon.Pun;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct HandledPickupDetails
{
    public HandledWeaponBase weapon;
    public Mesh weaponMesh;
}


public class HandledWeaponPickup : MonoBehaviour
{
    public HandledPickupDetails[] allWeaponPickups;

    private HandledWeaponBase weapon;

    private PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();

        if (!pView.IsMine)
        {
            return;
        }

        pView.RPC("RPC_AssignPickupProperties", RpcTarget.All, Random.Range(0, allWeaponPickups.Length));

    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 0.1f);
    }

    [PunRPC]
    void RPC_AssignPickupProperties(int index)
    {
        weapon = allWeaponPickups[index].weapon;
        GetComponent<MeshFilter>().mesh = allWeaponPickups[index].weaponMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EPlayerController>())
        {
            if (other.GetComponent<EPlayerController>().playerEnergy.energyPack)
            {
                //other.GetComponent<EPlayerController>().playerEnergy.energyPack.SetEnergyWeapon(weapon);
                other.GetComponent<EPlayerController>().PickedUpHandledWeapon(weapon);
                StartCoroutine(PickedUp());
            }
            else
            {
                other.GetComponent<EPlayerController>().playerUI.DisplayAlertMessage("Can't pickup without energy pack");
            }
        }
    }

    IEnumerator PickedUp()
    {
        pView.RPC("RPC_PickedUpEffectOnClients", RpcTarget.All);
        yield return new WaitForSeconds(0.1f);
        if (pView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void RPC_PickedUpEffectOnClients()
    {
        //ParticleSystem tPS = Instantiate(destructionEffect, transform.position, destructionEffect.transform.rotation);
        //tPS.GetComponent<ParticleSystemRenderer>().material.color = destructionColor;
        //tPS.GetComponent<ParticleSystemRenderer>().trailMaterial.color = destructionColor;
    }


}
