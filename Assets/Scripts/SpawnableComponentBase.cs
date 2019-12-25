using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableComponentBase : MonoBehaviour
{
    protected SpawnableGO parentSpawnable;

    public void Start()
    {
        parentSpawnable = GetComponent<SpawnableGO>();
    }
}
