using UnityEngine;

public class SpawnPad : MonoBehaviour
{
    public ChainPlatform parentPlatform;

    private void Start()
    {
        parentPlatform = GetComponentInParent<ChainPlatform>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EPlayerController>())
        {
            parentPlatform.PlatformTriggered(transform.GetSiblingIndex());
        }
    }
}
