using Photon.Pun;
using UnityEngine;

public class HandledWeaponBase : MonoBehaviour
{
    [Header("Weapon Specific Settings")]
    public Projectile projectile;
    public float durationBetwenShots;
    public float projectileSpeed;
    public int projectileDamage;
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

    public virtual void ShootEnergyWeapon() { }

}
