using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SpawnableHealth : SpawnableComponentBase
{
    //[Header("Dimensions")]
    //public Vector3 dimensions;


    [Header("Health Details")]
    public Material healthBar;
    public Renderer healthRenderer;

    private int health;
    private int currentHealth;
    private SpawnableGO master;
    private PhotonView pView;

    private bool jugaad_notEvaluated;

    private void Awake()
    {
        master = GetComponentInParent<SpawnableGO>();
        pView = GetComponent<PhotonView>();
    }
    new void Start()
    {
        base.Start();
        jugaad_notEvaluated = false;
    }

    public void TakeDamage(int damageAmount, int id)
    {
        if (!pView.IsMine)
        {
            return;
        }
        pView.RPC("RPC_TakeDamage", RpcTarget.All, damageAmount, id);
    }

    [PunRPC]
    public void RPC_TakeDamage(int damageAmount, int id)
    {
        currentHealth -= damageAmount;
        master.GetAppearanceComponent().WasHit();
        if (currentHealth <= 0)
        {

            if (!jugaad_notEvaluated)
            {
                jugaad_notEvaluated = true;
                DestroySpawnable(id);
            }

        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.SetFloat("_HealthPercentage", ((float)currentHealth / (float)health));
    }

    public void ProjectileCollided(Projectile proj, int ownerID)
    {
        if (!master.isUsable)
        {
            return;
        }
        TakeDamage(proj.GetDamage(), proj.GetOwnerID());
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

    public void DestroySpawnable(int id)
    {
        if (GetComponent<OffensiveControllerBase>() && GetComponent<OffensiveControllerBase>().GetIsOccupied())
        {
            GetComponent<OffensiveControllerBase>().ForceEjectPlayer();
        }

        AlsoDestroyChildren(id);

        PhotonView.Find(id).GetComponent<EPlayerNetworkPresence>().BrokeSpawnable();

        Fragmenter frag = gameObject.AddComponent<Fragmenter>();
        frag.FragmentThisSpawnable(GetComponent<SpawnableGO>().kindOfSpawnable);

        if (pView.IsMine)
        {
            StartCoroutine(DelayedNetworkDestroy());
        }
    }

    IEnumerator DelayedNetworkDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.Destroy(transform.gameObject);
    }

    void AlsoDestroyChildren(int id)
    {
        SpawnableHealth[] allSpawnables = GetComponentsInChildren<SpawnableHealth>();
        if (allSpawnables.Length == 1)
        {
            return;
        }

        foreach(SpawnableHealth curr in allSpawnables)
        {
            if (curr == this)
            {
                continue;
            }
            curr.DestroySpawnable(id);
        }
    }
}