using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float homingSpeed;
    private EPlayerController target;

    public void SetTarget(EPlayerController t)
    {
        target = t;
        Propel();
    }
    public void Propel()
    {
        if (!target)
        {
            return;
        }
        transform.LookAt(target.transform);
        GetComponent<Rigidbody>().velocity = transform.forward * homingSpeed;
    }
}
