using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWeaponBase : MonoBehaviour
{
    public Projectile projectile;
    public float durationBetwenShots;
    public float projectileSpeed;
    public int projectileDamage;
    public int projectileCost;
    public int beamsRequired;

    protected EPlayerController player;
    protected Projectile spawnedProjectile;
    protected EnergyPack energySource;

    public void SetOwner(EPlayerController tPlayer)
    {
        player = tPlayer;
    }

    public void SetEnergySource(EnergyPack source)
    {
        energySource = source;
    }

    public virtual void ShootEnergyWeapon() { }


}
