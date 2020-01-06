using Photon.Pun;
using System.IO;
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

    public void SpawnEnergyProjectile()
    {
        spawnedProjectile = PhotonNetwork.Instantiate(Path.Combine(projectile.pathStrings), energySource.nozzle.transform.position, energySource.playerCam.transform.rotation).GetComponent<Projectile>();
        spawnedProjectile.SetDamage(projectileDamage);
        spawnedProjectile.GetComponent<Rigidbody>().velocity = spawnedProjectile.transform.forward * projectileSpeed;
    }

    public virtual void ShootEnergyWeapon() { }


}
