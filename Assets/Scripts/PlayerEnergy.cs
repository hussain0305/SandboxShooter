using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    const int MAX_ENERGY = 1000;
    const int ENERGY_PULSE_AMOUNT = 10;
    const int ENERGY_PULSE_DURATION = 4;

    [HideInInspector]
    public PlayerUI playerUI;
    [HideInInspector]
    public bool hasEnergyPack;
    
    public EnergyPack energyPack;
    public Transform energyPackHolder;

    private int currentEnergy;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        currentEnergy = MAX_ENERGY / 2;
        hasEnergyPack = true;

        StartCoroutine(GainPassiveEnergy());
    }

    public void GainEnergy(int amount)
    {
        currentEnergy += amount;
        playerUI.UpdateEnergyBar(currentEnergy);
    }

    public void SpendEnergy(int amount)
    {
        currentEnergy -= amount;
        playerUI.UpdateEnergyBar(currentEnergy);
    }

    IEnumerator GainPassiveEnergy()
    {
        //yield statement below is a fix/jugaad
        //setup script execution order so UI and energy components don't need to be manually and
        //sequentially enabled

        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            //add condition here so energy isn't gained while player is dead
            //don't add condition to while loop, so it doesn't need restarting every respawn

            currentEnergy += ENERGY_PULSE_AMOUNT;
            if(currentEnergy > MAX_ENERGY)
            {
                currentEnergy = MAX_ENERGY;
            }
            playerUI.UpdateEnergyBar(currentEnergy);
            yield return new WaitForSeconds(ENERGY_PULSE_DURATION);
        }
    }

    public void DropEnergyPack()
    {
        if (!hasEnergyPack)
        {
            return;
        }
        playerUI.SetEnergyDroppedMessage(true);
        GetComponent<PhotonView>().RPC("RPC_DropEnergyPack", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DropEnergyPack()
    {
        hasEnergyPack = false;
        energyPack.transform.SetParent(null);
        energyPack.WasDropped();
        energyPack = null;
    }

    public void PickupEnergyPack(int packID)//EnergyPack pack)
    {
        if (hasEnergyPack)
        {
            return;
        }
        playerUI.SetEnergyDroppedMessage(false);
        GetComponent<PhotonView>().RPC("RPC_PickupEnergyPack", RpcTarget.All, packID);
    }

    [PunRPC]
    public void RPC_PickupEnergyPack(int packID)
    {
        if (hasEnergyPack)
        {
            return;
        }

        hasEnergyPack = true;
        energyPack = PhotonView.Find(packID).GetComponent<EnergyPack>();
        energyPack.rBody.isKinematic = true;
        energyPack.pickupCollider.enabled = false;
        energyPack.transform.SetParent(energyPackHolder);
        energyPack.transform.SetPositionAndRotation(energyPackHolder.position, energyPackHolder.rotation);
        GetComponent<PlayerDisbalance>().ResetDisbalance();
    }







    #region Getters and Setters

    public int GetEnergy()
    {
        return currentEnergy;
    }

    #endregion
}
