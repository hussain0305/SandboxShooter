using System.Collections;
using System.IO;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public ParticleSystem hitEffect;
    public float disblanceImpact;
    
    private int damage;
    void Start()
    {
        //Makes the codebase more robust. But it is probably more efficient to just
        //be careful and assign the layer in the prefab, eliminating the need for assigning
        //every individual bullet
        //gameObject.layer = LayerMask.NameToLayer("Projectiles");

        StartCoroutine(DeathCountdown());
    }

    public void SetDamage(int dam)
    {
        damage = dam;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<SpawnableHealth>() && !collision.gameObject.GetComponent<TowerShield>())
        {
            collision.gameObject.GetComponentInParent<SpawnableHealth>().ProjectileCollided(this);
            StartCoroutine(LateDestroy(0.1f));
        }
        else if (collision.gameObject.GetComponent<PlayerBodyPartCollider>())//other.gameObject.GetComponent<ProxyHealth>() || other.gameObject.GetComponent<EPlayerController>())
        {
            StartCoroutine(LateDestroy(0.1f));
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void DestroyProjectile()
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public IEnumerator LateDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyProjectile();
    }
}
