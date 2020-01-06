using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerDisbalance : MonoBehaviour
{    
    private const float MAX_DISBALANCE = 1000;
    private const float BALANCE_RECOVERY_PERSEC = 100;

    private float currentDisbalance;
    private PlayerEnergy playerEnergy;
    private PlayerUI playerUI;

    private bool disbalanceActivityOngoing;
    public void Start()
    {
        currentDisbalance = 0;
        playerEnergy = GetComponent<PlayerEnergy>();
        playerUI = GetComponent<PlayerUI>();
        disbalanceActivityOngoing = false;
    }
    public void AddOnDisbalance(float disbalanceAmount)
    {
        currentDisbalance += disbalanceAmount;
        if (!disbalanceActivityOngoing)
        {
            StartCoroutine(BalanceRecovery());
        }
    }
   
    IEnumerator BalanceRecovery()
    {
        disbalanceActivityOngoing = true;
        while (currentDisbalance > 0)
        {
            playerUI.NewDisbalanceValueReceived(currentDisbalance / MAX_DISBALANCE);
            if (currentDisbalance >= MAX_DISBALANCE)
            {
                playerEnergy.DropEnergyPack();
                StopCoro();
            }
            currentDisbalance -= BALANCE_RECOVERY_PERSEC * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        disbalanceActivityOngoing = false;

    }

    void StopCoro()
    {
        StopAllCoroutines();
    }


}
