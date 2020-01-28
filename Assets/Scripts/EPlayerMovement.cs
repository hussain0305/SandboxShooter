using System.Collections;
using Photon.Pun;
using UnityEngine;

public class EPlayerMovement : MonoBehaviour
{
    const float NORMAL_GRAVITY = 20;
    const float REDUCED_GRAVITY = 1;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public bool isWallGliding;

    //[Header("Movement Settings")]
    private float forwardSpeed = 10;
    private float dashSpeed = 30;
    private float glideSpeed = 20;
    private float strafeSpeedMultiplier = 0.5f;
    private float jumpSpeed = 10;
    private float jumpRunThreshold = 2;
    private float gravity;

    [Header("Visuals")]
    public TrailRenderer trail;
    
    private bool playerInAir;
    private bool isUsingOffensive;
    private bool isDashing;
    private bool canWallJump;
    private float curveFactor;
    private float currentSpeed;
    private float axisHorizontal;
    private float axisVertical;
    private Coroutine dashingCoroutine;
    private Vector3 moveDirection;
    private Vector3 probeOffset;
    private CharacterController characterController;
    private Animator characterAnimator;
    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        isDashing = false;
        playerInAir = false;
        currentSpeed = forwardSpeed;
        isGrounded = true;
        canWallJump = true;
        isWallGliding = false;
    }

    private void Start()
    {
        StartCoroutine(GravityModificationRoutine());
        gravity = NORMAL_GRAVITY;

        probeOffset = new Vector3(0, 0.1f, 0);
    }

    public void Update()
    {
        if(!pView.IsMine || !characterController || isUsingOffensive)
        {
            return;
        }

        if (Input.GetButtonDown("Run") && !isDashing && (Input.GetAxis("Vertical") > 0))
        {
            DashPls();
        }

        if (Input.GetButtonUp("Run"))
        {
            StopDashingPls();
        }


        axisVertical = Input.GetAxis("Vertical");
        axisHorizontal = Input.GetAxis("Horizontal");

        #region Grounded Check
        if (Physics.Raycast(transform.position, -1 * transform.up, 0.005f))
        {
            isGrounded = true;
            if (!canWallJump)
            {
                canWallJump = true;
            }
        }
        else
        {
            isGrounded = false;
        }
        #endregion

        if (isGrounded)
        {
            if (playerInAir)
            {
                playerInAir = false;
                characterAnimator.SetTrigger("PlayerLanded");
            }
            moveDirection = currentSpeed * ((transform.forward * axisVertical) + (transform.right * strafeSpeedMultiplier * axisHorizontal));

            if (Input.GetButtonDown("Jump"))
            {
                if (Vector3.Magnitude(characterController.velocity) < jumpRunThreshold)
                {
                    characterAnimator.SetTrigger("StandingJump");
                }
                else
                {
                    characterAnimator.SetTrigger("RunningJump");
                }
                playerInAir = true;
                moveDirection.y = jumpSpeed;
            }
        }

        else if (Input.GetButtonDown("Jump") && isWallGliding && canWallJump)
        {
            canWallJump = false;
            moveDirection = 50 * transform.forward;
            moveDirection += 15 * transform.up;
        }
        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
        characterAnimator.SetFloat("VelRight", axisHorizontal);
        characterAnimator.SetFloat("VelFwd", axisVertical * (Input.GetButton("Run") ? 1 : 0.5f));
    }

    public void SetIsUsingOffensive(bool val)
    {
        isUsingOffensive = val;
    }

    public void DashPls()
    {
        if (isDashing)
        {
            StopCoroutine(dashingCoroutine);
        }
        dashingCoroutine = StartCoroutine(DashForward());
    }

    public void StopDashingPls()
    {
        if (isDashing)
        {
            StopCoroutine(dashingCoroutine);
        }
        dashingCoroutine = StartCoroutine(StopDashing());

    }

    IEnumerator DashForward()
    {
        isDashing = true;
        currentSpeed = 2;
        yield return new WaitForSeconds(0.15f);

        trail.enabled = true;
        while (currentSpeed <= (dashSpeed - 0.25f))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, dashSpeed, 40 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);
        trail.enabled = false;

        curveFactor = 0.5f;
        while (currentSpeed >= (glideSpeed + 0.25f))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, glideSpeed, curveFactor * Time.deltaTime);
            curveFactor += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        isDashing = false;
        
        currentSpeed = glideSpeed;
    }

    IEnumerator StopDashing()
    {
        isDashing = true;
        trail.enabled = false;

        curveFactor = 1.5f;
        while(currentSpeed >= (forwardSpeed + 0.25f))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, forwardSpeed, curveFactor * Time.deltaTime);
            curveFactor += 0.075f;
            yield return new WaitForEndOfFrame();
        }
        isDashing = false;
        currentSpeed = forwardSpeed;
    }

    IEnumerator GravityModificationRoutine()
    {
        while (true)
        {
            if (!isGrounded && Input.GetButton("Run") && (
                Physics.Raycast(transform.position + probeOffset, transform.right, 1, ~(1 << 13)) ||
                Physics.Raycast(transform.position + probeOffset, -1 * transform.right, 1, ~(1 << 13)) ||
                Physics.Raycast(transform.position + probeOffset, transform.forward, 1, ~(1 << 13)) ||
                Physics.Raycast(transform.position + probeOffset, -1 * transform.forward, 1, ~(1 << 13))))
            {
                if (gravity != REDUCED_GRAVITY)
                {
                    SetIsWallGliding(true);
                    SetGravity(REDUCED_GRAVITY);
                }
            }
            else
            {
                if (gravity != NORMAL_GRAVITY && isWallGliding)
                {
                    SetIsWallGliding(false);
                    SetGravity(NORMAL_GRAVITY);
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void AddToVelocity(Vector3 direction)
    {
        moveDirection += direction;
    }

    void SetGravity(float newGravity)
    {
        gravity = newGravity;
    }

    void SetIsWallGliding(bool glideState)
    {
        isWallGliding = glideState;
    }

}
