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
        transform.LookAt(target.transform);
        GetComponent<Rigidbody>().velocity = transform.forward * homingSpeed;
    }
}
