using System.Collections;
using Photon.Pun;
using UnityEngine;

public class EPlayerMovement : MonoBehaviour, IPunObservable
{
    const float NORMAL_GRAVITY = 20;
    const float REDUCED_GRAVITY = 1;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public bool isWallGliding;
    [HideInInspector]
    public PlayerRelativeDirection glidingDirection;

    [HideInInspector]
    public Camera playerCam;

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
    private PhotonTransformViewClassic pTransform;
    private PlayerExternalMovement externalMovement;

    private Vector3 predictionVel;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        pTransform = GetComponent<PhotonTransformViewClassic>();
        externalMovement = gameObject.AddComponent<PlayerExternalMovement>();
        SetExternalMovement(false);
        isDashing = false;
        playerInAir = false;
        currentSpeed = forwardSpeed;
        isGrounded = true;
        canWallJump = true;
        isWallGliding = false;
    }

    private void Start()
    {
        gravity = NORMAL_GRAVITY;
        probeOffset = new Vector3(0, 0.1f, 0);

        if (!pView.IsMine)
        {
            return;
        }

        StartCoroutine(GravityModificationRoutine());
        StartCoroutine(FetchCamera());
    }

    public void Update()
    {
        if(!pView.IsMine || !characterController)
        {
            return;
        }

        pTransform.SetSynchronizedValues(characterController.velocity, 0);

        if (isUsingOffensive)
        {
            if (characterAnimator.GetFloat("VelFwd") != 0 || characterAnimator.GetFloat("VelRight") != 0)
            {
                //pTransform.SetSynchronizedValues(new Vector3(0 ,0 ,0), 0);
                pView.RPC("RPC_StopOnOccupyingOffensive", RpcTarget.All);
            }
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
        if (Physics.Raycast(transform.position, -1 * transform.up, 0.02f))//0.005
        {
            isGrounded = true;
            if (!canWallJump)
            {
                canWallJump = true;
            }
        }
        else //if(isGrounded)
        {
            isGrounded = false;

        }
        #endregion

        if (isGrounded)
        {
            if (playerInAir)
            {
                playerInAir = false;
                pView.RPC("RPC_PlayAnimationOnClients", RpcTarget.All, "PlayerLanded");
            }
            moveDirection = currentSpeed * ((transform.forward * axisVertical) + (transform.right * strafeSpeedMultiplier * axisHorizontal));

            if (Input.GetButtonDown("Jump"))
            {
                if (Vector3.Magnitude(characterController.velocity) < jumpRunThreshold)
                {
                    pView.RPC("RPC_PlayAnimationOnClients", RpcTarget.All, "StandingJump");
                }
                else
                {
                    pView.RPC("RPC_PlayAnimationOnClients", RpcTarget.All, "RunningJump");
                }
                playerInAir = true;
                moveDirection.y = jumpSpeed;
            }
        }

        else if (Input.GetButtonDown("Jump") && isWallGliding && canWallJump)
        {
            canWallJump = false;
            moveDirection = 30 * playerCam.transform.forward;
            //moveDirection += 7.5f * transform.up;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        //pTransform.SetSynchronizedValues(characterController.velocity, 0);

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

    public void StopInherentMovement()
    {
        characterController.Move(Vector3.zero);
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
            RaycastHit probeHit;
            if (!isGrounded && Input.GetButton("Run"))            
            {
                if ((Physics.Raycast(transform.position + probeOffset, transform.right, out probeHit, 1, ~(1 << 13)) && 
                    (Mathf.Cos(Vector3.Dot(transform.up, probeHit.normal)) == 1)))
                {
                    glidingDirection = PlayerRelativeDirection.Right;
                    if (gravity != REDUCED_GRAVITY)
                    {
                        SetIsWallGliding(true);
                        SetGravity(REDUCED_GRAVITY);
                    }
                }
                else if((Physics.Raycast(transform.position + probeOffset, -1 * transform.right, out probeHit, 1, ~(1 << 13)) &&
                    (Mathf.Cos(Vector3.Dot(transform.up, probeHit.normal)) == 1)))
                {
                    glidingDirection = PlayerRelativeDirection.Left;
                    if (gravity != REDUCED_GRAVITY)
                    {
                        SetIsWallGliding(true);
                        SetGravity(REDUCED_GRAVITY);
                    }

                }
                else if ((Physics.Raycast(transform.position + probeOffset, transform.forward, out probeHit, 1, ~(1 << 13)) &&
                    (Mathf.Cos(Vector3.Dot(transform.up, probeHit.normal)) == 1)))
                {
                    glidingDirection = PlayerRelativeDirection.Front;
                    if (gravity != REDUCED_GRAVITY)
                    {
                        SetIsWallGliding(true);
                        SetGravity(REDUCED_GRAVITY);
                    }
                }
                else if ((Physics.Raycast(transform.position + probeOffset, -1 * transform.forward, out probeHit, 1, ~(1 << 13)) &&
                    (Mathf.Cos(Vector3.Dot(transform.up, probeHit.normal)) == 1)))
                {
                    glidingDirection = PlayerRelativeDirection.Back;
                    if (gravity != REDUCED_GRAVITY)
                    {
                        SetIsWallGliding(true);
                        SetGravity(REDUCED_GRAVITY);
                    }
                }
                else
                {
                    CheckForGravityReversal();
                }
            }

            else
            {
                CheckForGravityReversal();
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    void CheckForGravityReversal()
    {
        if (gravity != NORMAL_GRAVITY && isWallGliding)
        {
            SetIsWallGliding(false);
            SetGravity(NORMAL_GRAVITY);
        }
    }

    public void AddToVelocity(Vector3 direction)
    {
        moveDirection += direction;
    }

    public void SetGravity(float newGravity)
    {
        gravity = newGravity;
    }

    public void RevertGravity()
    {
        SetGravity(NORMAL_GRAVITY);
    }

    void SetIsWallGliding(bool glideState)
    {
        isWallGliding = glideState;
    }

    [PunRPC]
    void RPC_PlayAnimationOnClients(string triggerName)
    {
        characterAnimator.SetTrigger(triggerName);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(axisHorizontal);
            stream.SendNext(axisVertical * (Input.GetButton("Run") ? 1 : 0.5f));
        }
        else if (stream.IsReading)
        {
            characterAnimator.SetFloat("VelRight", (float)stream.ReceiveNext());
            characterAnimator.SetFloat("VelFwd", (float)stream.ReceiveNext());
        }
    }

    [PunRPC]
    void RPC_StopOnOccupyingOffensive()
    {
        characterAnimator.SetFloat("VelRight", 0);
        characterAnimator.SetFloat("VelFwd", 0);
    }

    IEnumerator FetchCamera()
    {
        yield return new WaitForSeconds(0.5f);
        playerCam = GetComponent<EPlayerController>().playerCamera;
    }


    public void SetExternalMovement(bool val)
    {

        externalMovement.enabled = val;
    }
}
