using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    public SpawnableType type;

    private EPlayerController owner;
    private DefensiveBase structure;

    private void Start()
    {
        StartCoroutine(DelayedAction());
    }

    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.layer = LayerMask.NameToLayer("ProximitySensor");
    }
    private void OnTriggerEnter(Collider other)
    {
        /*
         * DO NOT DELETE
         * 
        if (!owner || !GetComponentInParent<SpawnableGO>().isUsable)
        {
            return;
        }

        if (other.GetComponent<EPlayerController>() && other.GetComponent<EPlayerController>() != owner)
        {
            structure.OpponentDetected(other.GetComponent<EPlayerController>());
        }
        */
        if (other.GetComponent<TestEnemy>())
        {
            structure.OpponentDetected(other.GetComponent<TestEnemy>());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TestEnemy>())
        {
            structure.OpponentLeft(other.GetComponent<TestEnemy>());
        }
    }

    public void SetOwner(EPlayerController own)
    {
        owner = own;
    }

    public void SetStructure(DefensiveBase str)
    {
        structure = str;
    }
}
