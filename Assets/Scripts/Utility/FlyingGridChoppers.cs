using Photon.Pun;
using System.Collections;
using UnityEngine;

public class FlyingGridChoppers : MonoBehaviour
{
    const int damage = 5000;


    public void Start()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
        {
            return;
        }
        RemoveAllInArea();
    }

    public void RemoveAllInArea()
    {
        Vector3 pos = transform.position;

        Collider[] hits;

        int[] allSteps = new int[] { -10, -5, 0, 5, 10 };

        foreach (int currX in allSteps)
        {
            foreach (int currZ in allSteps)
            {
                hits = Physics.OverlapSphere((pos + new Vector3(currX, 0, currZ)), 1, (1 << LayerMask.NameToLayer("SpawnableGO")));
                if (hits.Length > 0)
                {
                    hits[0].transform.GetComponentInParent<SpawnableHealth>().TakeDamage(damage, (((GetComponentInParent<PhotonView>().ViewID / 1000) * 1000) + 1));
                }

            }
        }
    }

}
