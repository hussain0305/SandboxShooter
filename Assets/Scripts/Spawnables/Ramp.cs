using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public float launchSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EPlayerMovement>())
        {          
            other.GetComponent<EPlayerMovement>().AddToVelocity((transform.forward * launchSpeed) + (transform.up * launchSpeed));
        }
    }
}
