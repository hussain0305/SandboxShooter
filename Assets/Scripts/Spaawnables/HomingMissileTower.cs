using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileTower : DefensiveBase
{
    public Transform nozzle;
    public override void PerformAttack()
    {
        if (opponentsInVicinity.Count < 1)
        {
            return;
        }

        Projectile h = Instantiate(projectile, nozzle.position, nozzle.rotation);
        h.GetComponent<HomingMissile>().SetTarget(opponentsInVicinity[0]);
        StartCoroutine(AttackAgainAfter(frequency));
    }

    public override void StopAttack()
    {
        if (opponentsInVicinity.Count > 0)
        {

        }
    }

}
