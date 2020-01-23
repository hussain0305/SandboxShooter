using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingNozzle : MonoBehaviour
{
    public Transform target;

    public float speed = 10.0f;

    public bool shouldRotate = false;

    void Update()
    {
        if (!shouldRotate || !target)
        {
            return;
        }
        Vector3 targetDirection = target.position - transform.position;

        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
