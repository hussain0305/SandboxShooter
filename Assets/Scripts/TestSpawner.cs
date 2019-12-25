using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public Projectile projectile;
    public float fireAfterSeconds;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNow());
    }

    IEnumerator SpawnNow()
    {
        while (true)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            yield return new WaitForSeconds(fireAfterSeconds);
        }
    }
}
