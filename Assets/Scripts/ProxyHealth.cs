﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyHealth : MonoBehaviour
{
    public SpawnableHealth healthComponent;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            healthComponent.TakeDamage(collision.gameObject.GetComponent<Projectile>().GetDamage(),
                collision.gameObject.GetComponent<Projectile>().GetOwnerID());
        }

        if (collision.gameObject.GetComponent<WreckingBall>())
        {
            healthComponent.TakeDamage(collision.gameObject.GetComponent<WreckingBall>().GetDamage(),
                collision.gameObject.GetComponent<WreckingBall>().GetPusherID());
        }
    }
}
