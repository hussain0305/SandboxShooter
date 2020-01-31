using Photon.Pun;
using UnityEngine;

public class SpawnableHealth : SpawnableComponentBase
{
    //[Header("Dimensions")]
    //public Vector3 dimensions;


    [Header("Health Details")]
    public Material healthBar;
    public Renderer healthRenderer;

    [Header("Death")]
    public ParticleSystem destructionEffect;

    private int health;
    private int currentHealth;
    private SpawnableGO master;
    private PhotonView pView;

    private void Awake()
    {
        master = GetComponentInParent<SpawnableGO>();
        pView = GetComponent<PhotonView>();
    }
    new void Start()
    {
        base.Start();
    }

    public void TakeDamage(int damageAmount)
    {
        pView.RPC("RPC_TakeDamage", RpcTarget.All, damageAmount);
    }

    [PunRPC]
    public void RPC_TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        master.GetAppearanceComponent().WasHit();
        if (currentHealth <= 0)
        {
            if (GetComponent<OffensiveControllerBase>() && GetComponent<OffensiveControllerBase>().GetIsOccupied())
            {
                GetComponent<OffensiveControllerBase>().ForceEjectPlayer();
            }
            DestroySpawnable();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.SetFloat("_HealthPercentage", ((float)currentHealth / (float)health));
    }

    public void ProjectileCollided(Projectile proj)
    {
        if (!master.isUsable)
        {
            return;
        }
        TakeDamage(proj.GetDamage());
    }

    public void InitiateSystems(int healthFromBP)
    {
        pView = GetComponent<PhotonView>();
        if (pView.IsMine)
        {
            pView.RPC("RPC_InitiateSystems", RpcTarget.All, healthFromBP);
        }
    }

    [PunRPC]
    public void RPC_InitiateSystems(int healthFromBP)
    {
        health = healthFromBP;
        currentHealth = health;
        healthBar = new Material(healthBar);
        healthRenderer.material = healthBar;

        UpdateHealthBar();

    }

    public void DestroySpawnable()
    {
        Instantiate(destructionEffect, transform.position, destructionEffect.transform.rotation);
        if (pView.IsMine)
        {
            PhotonNetwork.Destroy(transform.gameObject);
        }
    }
}