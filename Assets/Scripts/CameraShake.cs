using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    const float CAM_TILT_SPEED = 10.0f;
    //CAMERA TILT ON GLIDING REMOVED BECAUSE IT DIDN'T FIT WITH THE MECHANICS OF THE GAME
    //HAVEN'T REMOVED THE CODE PARTS IN CASE IT IS INCLUDED AGAIN LATER IN SOME FORM


    public Animator cameraAnimator;
    [HideInInspector]
    public bool isShaking;

    [HideInInspector]
    public bool shouldTilt;
    [HideInInspector]
    public PlayerRelativeDirection currentGlidingDirection;

    private EPlayerMovement playerMovement;
    private Vector3 tiltedLookVector;
    private float tiltAngle;
    private RaycastHit probeHit;
    private PlayerRelativeDirection glidingDirection;
    private float glidingAngle;

    private void Start()
    {
        cameraAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<EPlayerMovement>();
        isShaking = false;
        shouldTilt = false;
        tiltedLookVector = Vector3.zero;
        StartCoroutine(TrackPlayerActivity());
    }


    IEnumerator TrackPlayerActivity()
    {
        while (true)
        {
            if (!isShaking)
            {
                IsMoving();
            }
            ApplyGlidingTilt();
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

    float GetTiltAngle()
    {
        tiltAngle = 20 - (Mathf.Abs(90 - playerMovement.glidingAngle) / 3.0f);
        return tiltAngle;
    }

    public void ApplyGlidingTilt()
    {
        if (playerMovement.isWallGliding)
        {
            if (!shouldTilt)
            {
                shouldTilt = true;
                StartCoroutine(TiltCameraRoutine());
            }
        }

        else
        {
            tiltedLookVector = Vector3.zero;
            shouldTilt = false;
        }
    }

    IEnumerator TiltCameraRoutine()
    {
        cameraAnimator.enabled = false;
        while (shouldTilt)
        {
            if ((Physics.Raycast(playerMovement.gameObject.transform.position, playerMovement.gameObject.transform.right, out probeHit, 1, ~(1 << 13)) &&
                    (Mathf.Cos(Vector3.Dot(playerMovement.gameObject.transform.up, probeHit.normal)) == 1)))
            {
                glidingDirection = PlayerRelativeDirection.Right;
                glidingAngle = Vector3.Angle(Vector3.Normalize(-playerMovement.gameObject.transform.forward), probeHit.normal);
            }
            else if ((Physics.Raycast(playerMovement.gameObject.transform.position, -1 * playerMovement.gameObject.transform.right, out probeHit, 1, ~(1 << 13)) &&
                (Mathf.Cos(Vector3.Dot(playerMovement.gameObject.transform.up, probeHit.normal)) == 1)))
            {
                glidingDirection = PlayerRelativeDirection.Left;
                glidingAngle = Vector3.Angle(Vector3.Normalize(-playerMovement.gameObject.transform.forward), probeHit.normal);
            }
            else if ((Physics.Raycast(playerMovement.gameObject.transform.position, playerMovement.gameObject.transform.forward, out probeHit, 1, ~(1 << 13)) &&
                (Mathf.Cos(Vector3.Dot(playerMovement.gameObject.transform.up, probeHit.normal)) == 1)))
            {
                glidingDirection = PlayerRelativeDirection.Front;
                glidingAngle = Vector3.Angle(Vector3.Normalize(-playerMovement.gameObject.transform.forward), probeHit.normal);
            }
            else if ((Physics.Raycast(playerMovement.gameObject.transform.position, -1 * playerMovement.gameObject.transform.forward, out probeHit, 1, ~(1 << 13)) &&
                (Mathf.Cos(Vector3.Dot(playerMovement.gameObject.transform.up, probeHit.normal)) == 1)))
            {
                glidingDirection = PlayerRelativeDirection.Back;
                glidingAngle = Vector3.Angle(Vector3.Normalize(-playerMovement.gameObject.transform.forward), probeHit.normal);
            }

            switch (glidingDirection)
            {
                case PlayerRelativeDirection.Back:
                    tiltedLookVector = Vector3.zero;
                    break;
                case PlayerRelativeDirection.Front:
                    tiltedLookVector = new Vector3(0, 40, 0);
                    break;
                case PlayerRelativeDirection.Right:
                    tiltedLookVector = new Vector3(0, 0, GetTiltAngle());
                    break;
                case PlayerRelativeDirection.Left:
                    tiltedLookVector = new Vector3(0, 0, -GetTiltAngle());
                    break;
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(tiltedLookVector), CAM_TILT_SPEED * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(RemoveTilt());
    }

    IEnumerator RemoveTilt()
    {
        while (!shouldTilt && Vector3.Distance(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z), tiltedLookVector) > 0.25f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(tiltedLookVector), CAM_TILT_SPEED * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        cameraAnimator.enabled = true;
    }
}