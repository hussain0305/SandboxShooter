using UnityEngine;

public class FallMode_Detector : MonoBehaviour
{
    public DontFall fallMode;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EPlayerController>())
        {
            fallMode.PlayerFell(other.GetComponent<EPlayerController>().GetNetworkID());
            other.GetComponent<EPlayerController>().PlayerIsDead(0);
        }
    }
}
