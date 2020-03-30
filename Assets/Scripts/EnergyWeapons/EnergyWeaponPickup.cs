using Photon.Pun;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct WeaponPickupDetails
{
    public EnergyWeaponBase weapon;
    public Color weaponColor;
}


public class EnergyWeaponPickup : MonoBehaviour
{
    public WeaponPickupDetails[] allWeaponPickups;
    public ParticleSystem destructionEffect;

    private EnergyWeaponBase weapon;
    private Color destructionColor;

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

    [PunRPC]
    void RPC_AssignPickupProperties(int index)
    {
        weapon = allWeaponPickups[index].weapon;
        destructionColor = allWeaponPickups[index].weaponColor;

        GetComponent<Renderer>().material.color = destructionColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EPlayerController>())
        {
            if (other.GetComponent<EPlayerController>().playerEnergy.energyPack)
            {
                //other.GetComponent<EPlayerController>().playerEnergy.energyPack.SetEnergyWeapon(weapon);
                other.GetComponent<EPlayerController>().PickedUpEnergyWeapon(weapon);
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
        ParticleSystem tPS = Instantiate(destructionEffect, transform.position, destructionEffect.transform.rotation);
        tPS.GetComponent<ParticleSystemRenderer>().material.color = destructionColor;
        tPS.GetComponent<ParticleSystemRenderer>().trailMaterial.color = destructionColor;
    }


}
