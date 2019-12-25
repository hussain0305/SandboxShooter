using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float homingSpeed;
    private TestEnemy target;

    public void SetTarget(TestEnemy t)
    {
        target = t;
        Propel();
    }
    public void Propel()
    {
        transform.LookAt(target.transform);
        GetComponent<Rigidbody>().velocity = transform.forward * homingSpeed;
    }
}
