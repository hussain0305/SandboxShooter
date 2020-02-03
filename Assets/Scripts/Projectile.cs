using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public ParticleSystem hitEffect;
    public float disblanceImpact;
    
    private int damage;

    private int ownerID;
    void Start()
    {
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
            collision.gameObject.GetComponentInParent<SpawnableHealth>().ProjectileCollided(this, ownerID);
            StartCoroutine(LateDestroy(0.025f));
        }
        else if (collision.gameObject.GetComponent<PlayerBodyPartCollider>())//other.gameObject.GetComponent<ProxyHealth>() || other.gameObject.GetComponent<EPlayerController>())
        {
            StartCoroutine(LateDestroy(0.025f));
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

    public int GetOwnerID()
    {
        return ownerID;
    }

    public void SetOwnerID(int id)
    {
        ownerID = id;
    }
}
