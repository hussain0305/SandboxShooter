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

        hasEnergyPack = false;
        energyPack.transform.SetParent(null);
        energyPack.WasDropped();
        energyPack = null;
    }

    public void PickupEnergyPack(EnergyPack pack)
    {
        if (hasEnergyPack)
        {
            return;
        }

        hasEnergyPack = true;
        energyPack = pack;
        pack.rBody.isKinematic = true;
        pack.pickupCollider.enabled = false;
        pack.transform.SetParent(energyPackHolder);
        pack.transform.SetPositionAndRotation(energyPackHolder.position, energyPackHolder.rotation);
    }



    #region Getters and Setters

    public int GetEnergy()
    {
        return currentEnergy;
    }

    #endregion
}
