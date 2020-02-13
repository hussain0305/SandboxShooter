using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OpenWorldGenerator : MonoBehaviour
{
    const float WEAPON_SPAWN_INTERVAL = 25;

    public string[] platformPath;
    public string[] weaponPickupPath;
    public int numPlatforms;

    private List<Vector3> platformLocations;

    private void Start()
    {
        platformLocations = new List<Vector3>();
        SpawnWorld();
    }
    void SpawnWorld()
    {
        int spawnX, spawnY, spawnZ;
        int initialDimension = numPlatforms / 3;
        Vector3 currPosition;
        for (int loop = 0; loop < numPlatforms; loop++)
        {
            spawnX = Random.Range(-initialDimension, initialDimension) * 50;
            spawnY = Random.Range(0, initialDimension) * 50;
            spawnZ = Random.Range(-initialDimension, initialDimension) * 50;
            currPosition = new Vector3(spawnX, spawnY, spawnZ);
            PhotonNetwork.Instantiate(Path.Combine(platformPath), currPosition, Quaternion.identity);
            platformLocations.Add(currPosition);
        }

        StartCoroutine(PickupRoutine());
    }

    IEnumerator PickupRoutine()
    {
        Vector3 chosenLocation, probeLocation;
        RaycastHit hitResult;
        while (true)
        {
            chosenLocation = platformLocations[Random.Range(0, platformLocations.Count)];
            probeLocation = chosenLocation + transform.up * 10;
            if (Physics.Raycast(probeLocation, -1 * transform.up, out hitResult, 12))
            {
                if (hitResult.transform.GetComponent<EnergyWeaponPickup>())
                {
                    PhotonNetwork.Destroy(hitResult.transform.gameObject);
                }
                PhotonNetwork.Instantiate(Path.Combine(weaponPickupPath), (chosenLocation + 2 * transform.up), Quaternion.identity);
            }
            yield return new WaitForSeconds(WEAPON_SPAWN_INTERVAL);
        }
    }

}
