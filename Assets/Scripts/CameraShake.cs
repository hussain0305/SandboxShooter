using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Animator cameraAnimator;

    public bool isShaking;

    private EPlayerMovement playerMovement;
    private void Start()
    {
        cameraAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<EPlayerMovement>();
        isShaking = false;

        StartCoroutine(CheckIfPlayerIsMoving());
    }

    IEnumerator CheckIfPlayerIsMoving()
    {
        while (true)
        {
            if (!isShaking)
            {
                IsMoving();
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void IsMoving()
    {
        isShaking = false;
        if (!playerMovement.isGrounded)
        {
            return;
        }
        if (Input.GetButton("Vertical") && Input.GetAxis("Vertical") > 0)
        {
            isShaking = true;
            if (Input.GetButton("Run"))
            {
                cameraAnimator.SetTrigger("IsRunning");
            }
            else
            {
                cameraAnimator.SetTrigger("IsWalking");
            }
        }
    }

}