using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Animator cameraAnimator;

    public bool isShaking;

    public bool isTilted;
    public PlayerRelativeDirection currentGlidingDirection;

    private EPlayerMovement playerMovement;
    private void Start()
    {
        cameraAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<EPlayerMovement>();
        isShaking = false;
        isTilted = false;

        StartCoroutine(CheckIfPlayerIsMoving());
        //StartCoroutine(CheckIfPlayerIsGliding());
    }

    IEnumerator CheckIfPlayerIsMoving()
    {
        while (true)
        {
            if (!isShaking)
            {
                IsMoving();
            }
            IsGliding();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator CheckIfPlayerIsGliding()
    {
        while (true)
        {
            IsGliding();

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void IsMoving()
    {
        isShaking = false;

        if (!playerMovement.isGrounded)
        {
            return;
        }
        if (Input.GetButton("Horizontal") || (Input.GetButton("Vertical") && Input.GetAxis("Vertical") > 0))
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

    public void IsGliding()
    {
        if (playerMovement.isWallGliding)
        {
            if (!isTilted)
            {
                currentGlidingDirection = playerMovement.glidingDirection;
                TiltCameraForGliding(currentGlidingDirection);
            }

            if(playerMovement.glidingDirection != currentGlidingDirection)
            {
                ReturnFromTiltCamera(currentGlidingDirection);
            }
        }

        else
        {
            if (isTilted)
            {
                ReturnFromTiltCamera(currentGlidingDirection);
            }
        }
    }

    public void TiltCameraForGliding(PlayerRelativeDirection direction)
    {
        switch (direction)
        {
            case PlayerRelativeDirection.Front:
                cameraAnimator.SetTrigger("GlideStraight");
                break;
            case PlayerRelativeDirection.Back:
                cameraAnimator.SetTrigger("GlideStraight");
                break;
            case PlayerRelativeDirection.Right:
                cameraAnimator.SetTrigger("GlideRight");
                break;
            case PlayerRelativeDirection.Left:
                cameraAnimator.SetTrigger("GlideLeft");
                break;
        }

        isTilted = true;
    }

    public void ReturnFromTiltCamera(PlayerRelativeDirection direction)
    {
        switch (direction)
        {
            case PlayerRelativeDirection.Front:
                cameraAnimator.SetTrigger("ReturnFromStraightTilt");
                break;
            case PlayerRelativeDirection.Back:
                cameraAnimator.SetTrigger("ReturnFromStraightTilt");
                break;
            case PlayerRelativeDirection.Right:
                cameraAnimator.SetTrigger("ReturnFromRightTilt");
                break;
            case PlayerRelativeDirection.Left:
                cameraAnimator.SetTrigger("ReturnFromLeftTilt");
                break;
        }

        isTilted = false;
    }

}