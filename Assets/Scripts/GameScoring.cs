using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoring : MonoBehaviour
{
    private List<PlayerRecord> playerRecords;

    private void Start()
    {
        playerRecords = new List<PlayerRecord>();
    }

    public void AddPlayerToList(EPlayerNetworkPresence player)
    {
        PlayerRecord currRecord = new PlayerRecord(player);
        playerRecords.Add(currRecord);
    }

    public void RegisterKill(EPlayerController deceased)
    {

    }

    public void RegisterKill(EPlayerController deceased, EPlayerController perpetrator)
    {
        foreach(PlayerRecord currRecord in playerRecords)
        {
            //if(currRecord.player == deceased.GetNetworkPresence())
            //{
            //    currRecord.deaths += 1;
            //}
        }
    }

}
