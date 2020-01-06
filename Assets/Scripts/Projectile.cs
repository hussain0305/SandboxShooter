using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public ParticleSystem hitEffect;
    public float disblanceImpact;

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
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        if (collision.gameObject.GetComponentInParent<SpawnableHealth>())
        {
            collision.gameObject.GetComponentInParent<SpawnableHealth>().ProjectileCollided(this);
            GetComponent<PhotonView>().RPC("RPC_ProjectileCollidedWithSomething", RpcTarget.All);
            StartCoroutine(LateDestroy());
        }
        else if (collision.gameObject.GetComponent<ProxyHealth>() || collision.gameObject.GetComponent<EPlayerController>())
        {
            GetComponent<PhotonView>().RPC("RPC_ProjectileCollidedWithSomething", RpcTarget.All);
            StartCoroutine(LateDestroy());
        }
    }

    [PunRPC]
    public void RPC_ProjectileCollidedWithSomething()
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
    }

    public int GetDamage()
    {
        return damage;
    }

    IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(lifetime);
        if (GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public IEnumerator LateDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.Destroy(gameObject);
    }
}
