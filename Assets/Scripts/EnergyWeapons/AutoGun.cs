using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGun : EnergyWeaponBase
{
    public override void ShootEnergyWeapon()
    {
        if (player.playerEnergy.GetEnergy() >= projectileCost)
        {
            player.playerEnergy.SpendEnergy(projectileCost);
            spawnedProjectile = Instantiate(projectile, energySource.nozzle.transform.position, energySource.playerCam.transform.rotation);
            spawnedProjectile.SetDamage(projectileDamage);
            spawnedProjectile.GetComponent<Rigidbody>().velocity = spawnedProjectile.transform.forward * projectileSpeed;
            StartCoroutine(DelayedShootFromEnergyPack());
        }
        else
        {
            player.playerUI.DisplayAlertMessage("Not enough energy to shoot");
            energySource.DisableAttackBeam();
        }
    }

    IEnumerator DelayedShootFromEnergyPack()
    {
        yield return new WaitForSeconds(durationBetwenShots);
        if (energySource.isShooting)
        {
            ShootEnergyWeapon();
        }
    }
}
