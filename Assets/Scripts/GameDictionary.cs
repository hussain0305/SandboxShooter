﻿using UnityEngine;

[System.Serializable]
public enum SpawnableType { Offence, Defence, Decoration };

[System.Serializable]
public enum PlayerRelativeDirection { Front, Back, Right, Left};

[System.Serializable]
public enum KindOfSpawnable { BurstGun, MachineGun, DistributedGun, Wall, HomingTower, Ramp, FlyingGrid, Tower, Trampoline, ChainPlatform};

[System.Serializable]
public struct FragmentColorInfo
{
    public KindOfSpawnable kindOfSpawnable;
    public Material bodyMaterial;
    public Material trailMaterial;
}



[System.Serializable]
public class PlayerRecord
{
    private EPlayerNetworkPresence player;
    private int deaths;
    private int kills;
    private int spawnablesBroken;

    public PlayerRecord(EPlayerNetworkPresence subject)
    {
        player = subject;
        deaths = 0;
        kills = 0;
        spawnablesBroken = 0;
    }

    public void SetPlayer(EPlayerNetworkPresence presence)
    {
        player = presence;
    }

    public void AddKill()
    {
        kills++;
    }

    public void AddDeath()
    {
        deaths++;
    }

    public void AddSpawnableBroken()
    {
        spawnablesBroken++;
    }

    public int GetKills()
    {
        return kills;
    }

    public int GetDeaths()
    {
        return deaths;
    }

    public int GetSpawnablesBroken()
    {
        return spawnablesBroken;
    }

    public EPlayerNetworkPresence GetNetworkPresence()
    {
        return player;
    }

    
}