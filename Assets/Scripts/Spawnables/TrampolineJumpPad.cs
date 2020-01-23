using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineJumpPad : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EPlayerMovement>())
        {
            PlayerJumpedOnPad(other.gameObject.GetComponent<EPlayerMovement>());
        }
    }

    void PlayerJumpedOnPad(EPlayerMovement playerMovement)
    {
        playerMovement.AddToVelocity(new Vector3(0, 75, 0));
    }
}
