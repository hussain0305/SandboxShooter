using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShield : MonoBehaviour
{
    const int MAX_HEALTH = 2;//2000;
    
    private int health;


    private void Start()
    {
        health = MAX_HEALTH;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            health -= collision.gameObject.GetComponent<Projectile>().GetDamage();
            collision.gameObject.GetComponent<Projectile>().DestroyProjectile();

            if (health < 0)
            {
                GetComponent<FragmentationEffect>().SpawnFragmentationEffect();
                Destroy(this.gameObject);
            }
        }
    }
}
