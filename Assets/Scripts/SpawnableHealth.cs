using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableHealth : SpawnableComponentBase
{
    [Header("Dimensions")]
    public Vector3 dimensions;


    [Header("Health Details")]
    public Material healthBar;

    [Header("Death")]
    public ParticleSystem destructionEffect;

    private int health;
    private int currentHealth;
    private SpawnableGO master;
    private Renderer healthRenderer;

    new void Start()
    {
        base.Start();
        master = GetComponentInParent<SpawnableGO>();
        //InitiateSystems();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        master.GetAppearanceComponent().WasHit();
        if (currentHealth <= 0)
        {
            DestroySpawnable();
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.SetFloat("_HealthPercentage", ((float)currentHealth / (float)health));
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!master.isUsable)
        {
            return;
        }
        if (collision.gameObject.GetComponent<Projectile>())
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().GetDamage());
        }
    }

    public void InitiateSystems(int healthFromBP)
    {
        health = healthFromBP;
        currentHealth = health;
        healthRenderer = GetComponent<Renderer>();
        healthBar = new Material(healthBar);
        healthRenderer.material = healthBar;

        UpdateHealthBar();
    }


    public void DestroySpawnable()
    {
        Instantiate(destructionEffect, transform.position, destructionEffect.transform.rotation);
        Destroy(transform.parent.gameObject);
    }
}