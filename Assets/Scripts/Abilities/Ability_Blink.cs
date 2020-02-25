using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Blink : MonoBehaviour
{
    const int BLINK_COST = 50;
    const float BLINK_DISTANCE = 55;
    const float BLINK_SPEED = 50;

    private bool canBlink;
    public EPlayerController player;
    public GameObject blinkMarker;
    public TrailRenderer trail;

    private Camera cam;
    private Vector3 markerPosition;
    private RaycastHit hit;
    private GameObject spawnedMarker;

    void Start()
    {
        cam = Camera.main;
        trail.enabled = false;
        spawnedMarker = null;
        canBlink = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine || !canBlink || player.GetIsUsingOffensive())
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (player.playerEnergy.hasEnergyPack)
            {
                if (player.playerEnergy.GetEnergy() >= BLINK_COST)
                {
                    spawnedMarker = Instantiate(blinkMarker);
                }
                else
                {
                    player.playerUI.DisplayAlertMessage("You don't have enough energy to blink");
                }
            }
            else
            {
                player.playerUI.DisplayAlertMessage("Can't blink without Energy Pack");
            }
            
        }

        
        if (spawnedMarker && Input.GetButton("Fire2"))
        {
            if (!spawnedMarker)
            {
                return;
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, BLINK_DISTANCE, ~(1 << LayerMask.NameToLayer("ProximitySensor") | 1 << LayerMask.NameToLayer("BuildingDetector"))))
            {
                markerPosition = hit.point;
            }
            else
            {
                markerPosition = cam.transform.position + (cam.transform.forward * BLINK_DISTANCE);
            }
            spawnedMarker.transform.SetPositionAndRotation(markerPosition, Quaternion.identity);
        }
        if (spawnedMarker && Input.GetButtonUp("Fire2"))
        {
            StartCoroutine(BlinkTravel());
        }
    }


    IEnumerator BlinkTravel()
    {
        canBlink = false;
        trail.enabled = true;
        player.playerEnergy.SpendEnergy(BLINK_COST);
        while (Vector3.Distance(player.transform.position, markerPosition) > 2)
        {
            player.transform.SetPositionAndRotation(Vector3.Lerp(player.transform.position, markerPosition, Time.deltaTime * BLINK_SPEED), player.transform.rotation);
            yield return new WaitForEndOfFrame();
        }
        Destroy(spawnedMarker);
        yield return new WaitForSeconds(0.5f);
        trail.enabled = false;
        canBlink = true;
    }
}
