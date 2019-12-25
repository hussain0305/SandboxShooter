using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public Rigidbody body;
    public Vector3 startingVelocity;

    private void Start()
    {
        body.velocity = startingVelocity;
    }
}
