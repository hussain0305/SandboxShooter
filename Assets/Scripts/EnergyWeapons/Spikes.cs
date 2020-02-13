using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Spikes : EnergyWeaponBase
{
    public override void ShootEnergyWeapon()
    {
        if (player.playerEnergy.GetEnergy() >= projectileCost)
        {
            player.playerEnergy.SpendEnergy(projectileCost);

            SpawnEnergyProjectile();
        }
        else
        {
            player.playerUI.DisplayAlertMessage("Not enough energy to shoot");
        }
    }

    public void SpawnEnergyProjectile()
    {
        pView.RPC("RPC_SpawnEnergyProjectile", RpcTarget.All, (player.transform.position + (player.transform.forward * 30)),
            player.transform.rotation,
            player.GetNetworkID());
    }

    [PunRPC]
    void RPC_SpawnEnergyProjectile(Vector3 pos, Quaternion rot, int id)
    {
        StartCoroutine(SpawnSpikesAtInterval(pos, rot, id));
    }

    IEnumerator SpawnSpikesAtInterval(Vector3 pos, Quaternion rot, int id)
    {
        Vector3 modPos = pos;
        for (int loop = 0; loop < 10; loop++)
        {
            spawnedProjectile = Instantiate(projectile, modPos, rot).GetComponent<Projectile>();
            spawnedProjectile.SetDamage(projectileDamage);
            spawnedProjectile.GetComponent<Rigidbody>().velocity = (spawnedProjectile.transform.up * projectileSpeed);
            spawnedProjectile.SetOwnerID(id);
            modPos += spawnedProjectile.transform.forward * 5;
            yield return new WaitForSeconds(0.25f);
        }

    }


}
