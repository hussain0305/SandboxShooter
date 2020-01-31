using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPartCollider : MonoBehaviour
{
    private EPlayerController playerController;
    void Start()
    {
        playerController = GetComponentInParent<EPlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            if (playerController.IsLocalPView())
            {
                playerController.BodyPartHit(collision.gameObject.GetComponent<Projectile>().disblanceImpact);
            }
        }
    }


}
