using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWeaponPickup : MonoBehaviour
{
    public EnergyWeaponBase weapon;
    public ParticleSystem destructionEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EPlayerController>())
        {
            if (other.GetComponent<EPlayerController>().playerEnergy.energyPack)
            {
                other.GetComponent<EPlayerController>().playerEnergy.energyPack.SetEnergyWeapon(Instantiate(weapon));
                Instantiate(destructionEffect, transform.position, destructionEffect.transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                other.GetComponent<EPlayerController>().playerUI.DisplayAlertMessage("Can't pickup without energy pack");
            }
        }
    }
}
