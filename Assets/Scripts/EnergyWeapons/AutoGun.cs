using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AutoGun : EnergyWeaponBase
{

    public override void ShootEnergyWeapon()
    {
        if (player.playerEnergy.GetEnergy() >= projectileCost)
        {
            player.playerEnergy.SpendEnergy(projectileCost);
            
            SpawnEnergyProjectile();
            StartCoroutine(DelayedShootFromEnergyPack());
        }
        else
        {
            player.playerUI.DisplayAlertMessage("Not enough energy to shoot");
        }
    }

    IEnumerator DelayedShootFromEnergyPack()
    {
        yield return new WaitForSeconds(durationBetwenShots);
        if (energySource.isShootingEnergy)
        {
            ShootEnergyWeapon();
        }
    }

    public void SpawnEnergyProjectile()
    {
        pView.RPC("RPC_SpawnEnergyProjectile", RpcTarget.All, energySource.nozzle.transform.position,
            energySource.playerCam.transform.rotation,
            player.GetComponent<CharacterController>().velocity,
            player.GetNetworkID());
    }

    [PunRPC]
    void RPC_SpawnEnergyProjectile(Vector3 pos, Quaternion rot, Vector3 vel, int id)
    {
        spawnedProjectile = Instantiate(projectile, pos, rot).GetComponent<Projectile>();
        spawnedProjectile.SetDamage(projectileDamage);
        spawnedProjectile.GetComponent<Rigidbody>().velocity = (spawnedProjectile.transform.forward * projectileSpeed) + vel;
        spawnedProjectile.SetOwnerID(id);
    }
}
