using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    //Will be used when other weapons are added
    //public string[] allPickups;

    //private PhotonView pView;

    public GameObject rayStart;
    public void Start()
    {
        //if (GetComponent<PhotonView>() && GetComponent<PhotonView>().IsMine)
        //{
        //    pView = GetComponent<PhotonView>();
        //    StartCoroutine(SpawnWeapon());
        //}
        StartCoroutine(SpawnWeapon());
    }

    IEnumerator SpawnWeapon()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayStart.transform.position, -1 * transform.up, out hit, 25))
            {
                if (hit.collider.GetComponent<EnergyWeaponPickup>())
                {

                }
                else
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "EnergyWeaponPickup"), transform.position, transform.rotation, 0);
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

}
