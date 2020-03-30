using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class WeaponSpawner : MonoBehaviour
{
    public bool spawnEnergyWeapons;
    public bool spawnHandledWeapons;
    private PhotonView pView;

    public GameObject rayStart;
    public void Start()
    {
        pView = GetComponent<PhotonView>();
        if (pView.IsMine)
        {
            StartCoroutine(SpawnWeapon());
        }
    }

    IEnumerator SpawnWeapon()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayStart.transform.position, -1 * transform.up, out hit, 25))
            {
                if (spawnEnergyWeapons)
                {
                    if (hit.collider.GetComponent<EnergyWeaponPickup>())
                    {

                    }
                    else
                    {
                        EnergyWeaponPickup spawnedPickup = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "EnergyWeaponPickup"), transform.position, transform.rotation, 0).GetComponent<EnergyWeaponPickup>();

                    }
                }
                else if (spawnHandledWeapons)
                {
                    if (hit.collider.GetComponent<HandledWeaponPickup>())
                    {

                    }
                    else
                    {
                        HandledWeaponPickup spawnedPickup = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "HandledWeaponPickup"), transform.position, transform.rotation, 0).GetComponent<HandledWeaponPickup>();

                    }

                }
            }
            yield return new WaitForSeconds(30);
        }
    }
}
