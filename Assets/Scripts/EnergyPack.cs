﻿using Photon.Pun;
using System.Collections;
using UnityEngine;

public class EnergyPack : MonoBehaviour
{
    public Rigidbody rBody;
    public Collider pickupCollider;

    public Material energyMaterial;

    public GameObject nozzle;
    public Color activeColor;
    public Color inactiveColor;
    
    [HideInInspector]
    public Camera playerCam;

    [HideInInspector]
    public bool isShootingEnergy;
    
    private bool hasEnergyWeapon;
    private Transform owner;
    private EnergyWeaponBase currentEnergyWeapon;
    private EPlayerController player;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        energyMaterial = new Material(energyMaterial);
        GetComponent<Renderer>().material = energyMaterial;

        energyMaterial.SetColor("_Color", activeColor);
    }

    private void OnEnable()
    {
        owner = transform.parent.root;
        player = owner.GetComponent<EPlayerController>();
        playerCam = Camera.main;
        isShootingEnergy = false;
        hasEnergyWeapon = false;
    }

    void Update()
    {
        if (!owner || !pView.IsMine)
        {
            return;
        }
        if(Input.GetButtonDown("Fire1") && !player.handledWeapons.isADSing && player.IsInActiveGameplay())//EnergyWeaponUse
        {
            if (player.playerEnergy.hasEnergyPack)
            {
                if (hasEnergyWeapon)
                {
                    isShootingEnergy = true;
                    currentEnergyWeapon.ShootEnergyWeapon();
                }
                else
                {
                    player.playerUI.DisplayAlertMessage("Energy weapon not equipped");
                }
            }
            else
            {
                player.playerUI.DisplayAlertMessage("Can't shoot without energy pack");
            }
        }

        if(isShootingEnergy && Input.GetButton("Fire1"))//EnergyWeaponUse
        {

        }

        if (isShootingEnergy && Input.GetButtonUp("Fire1"))//EnergyWeaponUse
        {
            isShootingEnergy = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!owner || ! pView.IsMine)
        {
            return;
        }
        if (other.GetComponent<PlayerEnergy>() && other.GetComponent<PlayerEnergy>().transform == owner)
        {
            //other.GetComponent<PlayerEnergy>().PickupEnergyPack(this);
            other.GetComponent<PlayerEnergy>().PickupEnergyPack(GetComponent<PhotonView>().ViewID);
        }
    }

    public void WasDropped()
    {
        pView.RPC("RPC_WasDropped", RpcTarget.All);
    }

    [PunRPC]
    void RPC_WasDropped()
    {
        energyMaterial.SetColor("_Color", inactiveColor);
        rBody.isKinematic = false;
        StartCoroutine(Countdown());
    }

    public void SetEnergyWeapon(EnergyWeaponBase weapon)
    {
        hasEnergyWeapon = true;
        currentEnergyWeapon = weapon;
        weapon.SetOwner(player);
        weapon.SetEnergySource(this);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(5);
        energyMaterial.SetColor("_Color", activeColor);
        pickupCollider.enabled = true;
    }
}
