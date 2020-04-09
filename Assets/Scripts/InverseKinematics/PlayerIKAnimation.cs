using System.Collections;
using UnityEngine;

public class PlayerIKAnimation : MonoBehaviour
{
    [Header("IK Animation Poses/Profiles")]
    public Animator ikLegsAnimator;
    public Animator ikHandsAnimator;
    public IKMarkers idle;
    public IKMarkers walk;
    public IKMarkers run;

    [Header("Limb Targets")]
    public Transform handLeftTarget;
    public Transform handRightTarget;
    public Transform legLeftTarget;
    public Transform legRightTarget;

    [Header("Limb Poles")]
    public Transform handLeftPole;
    public Transform handRightPole;
    public Transform legLeftPole;
    public Transform legRightPole;

    [Header("Spine")]
    public Transform spineBone;
    public Transform spineRotationRef;

    private bool spineCoroutineRunning = false;
    private Quaternion originalSpineRotation;
    private EMovementState currentMovementState;
    private Coroutine spineCoroutine;

    private void Awake()
    {
        originalSpineRotation = spineBone.rotation;
    }
    private void Start()
    {
        StartCoroutine(SetIKParameters(EMovementState.Idle));
        StartCoroutine(IKAnimationUpdate());
    }

    IEnumerator IKAnimationUpdate()
    {
        while (true) {

            if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                if(Input.GetButton("Run") && currentMovementState != EMovementState.Running)
                {
                    StartCoroutine(SetIKParameters(EMovementState.Running));
                }
                else if(!Input.GetButton("Run") && currentMovementState != EMovementState.Walking)
                {
                    StartCoroutine(SetIKParameters(EMovementState.Walking));
                } 
            }
            else
            {
                if (currentMovementState != EMovementState.Idle)
                {
                    StartCoroutine(SetIKParameters(EMovementState.Idle));
                }
            }
            yield return new WaitForSeconds(1.0f / 10);
        }
    }

    IEnumerator SetIKParameters(EMovementState movementState)
    {
        currentMovementState = movementState;
        UpdateSpineMovement();
        yield return new WaitForSeconds(0.05f);

        switch (movementState)
        {
            case EMovementState.Idle:
                ikLegsAnimator.SetTrigger("Idle");
                ikHandsAnimator.SetTrigger("Idle");
                //yield return new WaitForSeconds(0.05f);
                ikLegsAnimator.enabled = false;
                ikHandsAnimator.enabled = false;

                //handLeftPole.position = idle.pole_handLeft.position;
                //handRightPole.position = idle.pole_handRight.position;

                //HAVEN'T ENCOUNTERED THE NEED TO MODIFY LEG POLES...YET
                //legLeftPole.position = idle.pole_legLeft.position;
                //legRightPole.position = idle.pole_legRight.position;

                handLeftTarget.position = idle.target_handLeft.position;                
                handRightTarget.position = idle.target_handRight.position;               
                legLeftTarget.position = idle.target_legLeft.position;             
                legRightTarget.position = idle.target_legRight.position;
                break;

            case EMovementState.Walking:
                ikLegsAnimator.enabled = true;
                ikHandsAnimator.enabled = true;
                //yield return new WaitForSeconds(0.05f);
                ikLegsAnimator.SetTrigger("Walk");
                ikHandsAnimator.SetTrigger("Walk");
                break;

            case EMovementState.Running:
                ikLegsAnimator.enabled = true;
                ikHandsAnimator.enabled = true;
                //yield return new WaitForSeconds(0.05f);
                ikLegsAnimator.SetTrigger("Run");
                ikHandsAnimator.SetTrigger("Run");
                break;

        }
    }


    public void UpdateSpineMovement()
    {
        if(currentMovementState == EMovementState.Idle)
        {
            if (spineCoroutineRunning)
            {
                StopCoroutine(spineCoroutine);
            }
            spineCoroutineRunning = false;
            spineBone.rotation = originalSpineRotation;
        }
        else
        {
            spineCoroutineRunning = true;
            spineCoroutine = StartCoroutine(SyncSpineRotation());
        }
    }

    IEnumerator SyncSpineRotation()
    {
        while (true)
        {
            spineBone.position = spineRotationRef.position;
            spineBone.rotation = spineRotationRef.rotation;
            yield return new WaitForSeconds(0.1f);
        }
    }
}


//if(currentMovementState != EMovementState.Walking || currentMovementState != EMovementState.Running)
//                {
//                    StartCoroutine(SetIKParameters(EMovementState.Walking));
//                }
//                if (Input.GetButton("Run"))
//                {
//                    ikAnimator.speed = 3;
//                }
//                else
//                {
//                    ikAnimator.speed = 1;
//                }