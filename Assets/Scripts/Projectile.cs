using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public ParticleSystem hitEffect;

    public string[] pathStrings;
    
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
        if (collision.gameObject.GetComponent<SpawnableHealth>())
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        else if (collision.gameObject.GetComponent<ProxyHealth>())
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
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
}
