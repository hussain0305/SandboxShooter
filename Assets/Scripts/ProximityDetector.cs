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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!owner || !GetComponentInParent<SpawnableGO>().isUsable)
    //    {
    //        return;
    //    }

    //    if (other.GetComponent<EPlayerController>() && other.GetComponent<EPlayerController>() != owner)
    //    {
    //        structure.OpponentDetected(other.GetComponent<EPlayerController>());
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<EPlayerController>())
    //    {
    //        structure.OpponentLeft(other.GetComponent<EPlayerController>());
    //    }
    //}

    public void SetOwner(EPlayerController own)
    {
        owner = own;
    }

    public void SetStructure(DefensiveBase str)
    {
        structure = str;
    }
}
