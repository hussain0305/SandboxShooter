using Photon.Pun;
using System.IO;
using UnityEngine;

public class EnergyWeaponBase : MonoBehaviour
{
    [Header("Weapon Specific Settings")]
    public bool requiresBeams;
    
    public Projectile projectile;
    public float durationBetwenShots;
    public float projectileSpeed;
    public int projectileDamage;
    public int projectileCost;
    public int beamsRequired;
    public string[] pathStrings;

    protected EPlayerController player;
    protected Projectile spawnedProjectile;
    protected EnergyPack energySource;
    protected PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
    }
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
