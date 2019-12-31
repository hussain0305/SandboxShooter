using Photon.Pun;
using UnityEngine;

//RUNNING : Player can select between toggling and holding the run button

public class EPlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerUI playerUI;
    public PlayerEnergy playerEnergy;

    [Header("Game Settings")]
    public float mouseSensitivity = 50;

    [Header("Player Settings")]
    public float forwardSpeed = 6;
    public float runningSpeed = 12;
    public float strafeSpeedMultiplier = 0.5f;
    public float jumpSpeed = 5f;
    public float jumpRunThreshold = 2;
    public float gravity = 10;

    [Header("Camera")]
    public Camera playerCamera;


    const float BUILD_DISTANCE = 20; 
    private bool constructionMenuOpen;
    private bool playerInAir;
    private bool isUsingOffensive;
    private float axisHorizontal;
    private float axisVertical;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private Vector3 camPosition;
    private Vector3 moveDirection;
    private Quaternion camRotation;
    private Transform playerBody;
    private Animator characterAnimator;
    private GameManager gameManager;
    private CharacterController characterController;
    private OffensiveControllerBase controlledOffensive;
    private EPlayerNetworkPresence networkPresence;

    private PhotonView pView;

    void Awake()
    {
        FetchComponents();
        InitializeValues();

        //Cursor.lockState = CursorLockMode.Confined;
    }

    void FetchComponents()
    {
        playerBody = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        networkPresence = GetComponent<EPlayerNetworkPresence>();
        //playerCamera = Camera.main;

        playerUI.enabled = true;
        playerEnergy.enabled = true;

        pView = GetComponent<PhotonView>();

        if (pView.IsMine)
        {
            playerCamera.gameObject.SetActive(true);
        }
    }

    void InitializeValues()
    {
        moveDirection = Vector3.zero;
        camPosition = playerCamera.transform.localPosition;
        camRotation = playerCamera.transform.localRotation;

        constructionMenuOpen = false;
        playerInAir = false;
        isUsingOffensive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine)
        {
            return;
        }

        #region Using offensive input
        if (Input.GetButtonDown("Use"))
        {
            if (controlledOffensive && !isUsingOffensive && !constructionMenuOpen)
            {
                OccupyOffensive();
            }

            else if (controlledOffensive && isUsingOffensive)
            {
                LeaveOffensive();
            }
        }
        #endregion

        if (!characterAnimator || !characterController || isUsingOffensive)
        {
            return;
        }

        axisVertical = Input.GetAxis("Vertical");
        axisHorizontal = Input.GetAxis("Horizontal");

        if (!constructionMenuOpen)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45f, 15f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        #region Construction Input
        if (Input.GetButtonDown("Build") && !isUsingOffensive)
        {
            if (playerEnergy.hasEnergyPack)
            {
                if (PlayerIsAimingCloseEnough()) 
                {
                    ToggleConstructionMenu();
                }
                else
                {
                    playerUI.DisplayAlertMessage("Aim closer on the ground to construct");
                }
            }
            else
            {
                playerUI.DisplayAlertMessage("Cannot construct without energy pack");
            }
        }
        #endregion

        #region Movement Input
        if (characterController.isGrounded)
        {
            if (playerInAir)
            {
                playerInAir = false;
                characterAnimator.SetTrigger("PlayerLanded");
            }
            moveDirection = (Input.GetButton("Run") ? runningSpeed : forwardSpeed) * ((transform.forward * axisVertical) + (transform.right * strafeSpeedMultiplier * axisHorizontal));

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

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
        characterAnimator.SetFloat("VelRight", axisHorizontal);
        characterAnimator.SetFloat("VelFwd", axisVertical * (Input.GetButton("Run") ? 1 : 0.5f));
        #endregion
    }

    #region Construction Stuff
    public void ToggleConstructionMenu()
    {
        constructionMenuOpen = !constructionMenuOpen;
        playerUI.ToggleConstructionMenu();
    }

    bool PlayerIsAimingCloseEnough()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, BUILD_DISTANCE))
        {
            if (hit.collider.tag == "GridFloor")
            {
                return true;
            }
        }
        return false;
    }

    public bool GetSpawnLocationAndRotation(out Vector3 location, out Quaternion rotation)
    {
        Vector3 loc = new Vector3(0, 0, 0);
        Quaternion rot = Quaternion.identity;
        location = loc;
        rotation = rot;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, BUILD_DISTANCE))
        {
            if (hit.collider.tag.Equals("GridFloor"))
            {
                if(AlignOnGrid(hit.point, out location))
                {
                    rotation = transform.rotation;
                    return true;
                }
                else
                {
                    playerUI.DisplayAlertMessage("A structure already exists here");
                }
            }
            else
            {
                playerUI.DisplayAlertMessage("Aim on the floor to construct");
            }
        }
        else
        {
            playerUI.DisplayAlertMessage("Aim somewhere nearby to construct");
        }
        return false;
    }

    bool AlignOnGrid(Vector3 rawCoordinates, out Vector3 location)
    {
        //IF THE GRID IS MODIFIED, THIS FORMULA NEEDS REDOING

        Vector3 gridLoc = new Vector3(0, rawCoordinates.y, 0);
        int mulX = ((int)rawCoordinates.x) / 5;
        int mulZ = ((int)rawCoordinates.z) / 5;
        gridLoc.x = (rawCoordinates.x < 0)? (mulX * 5) - 2.5f : (mulX * 5) + 2.5f;
        gridLoc.z = (rawCoordinates.z < 0) ? (mulZ * 5) - 2.5f : (mulZ * 5) + 2.5f;
        location = gridLoc;

        return !gameManager.ObjectExistsAtLocation(location);
    }
    #endregion

    void OccupyOffensive()
    {
        isUsingOffensive = true;
        controlledOffensive.OffensiveOccuppied(this);        
        playerUI.RemoveInstructionMessage();
    }

    void LeaveOffensive()
    {
        isUsingOffensive = false;
        playerCamera.transform.SetParent(this.transform);
        playerCamera.transform.localPosition = camPosition;
        playerCamera.transform.localRotation = camRotation;
        controlledOffensive.OffensiveLeft();
        SetControlledOffensive();
    }

    public bool IsInActiveGameplay()
    {
        //function to see whether the player is in active gameplay or
        //has any UI/menu open. keep adding to the condition as more
        //UI artifacts are added.

        return !(constructionMenuOpen || isUsingOffensive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() && GetComponent<PhotonView>().IsMine)
        {
            playerEnergy.DropEnergyPack();
        }
    }

    #region Getters and Setters

    public OffensiveControllerBase GetControlledOffensive()
    {
        return controlledOffensive;
    }

    public void SetControlledOffensive(OffensiveControllerBase controllee)
    {
        controlledOffensive = controllee;
    }

    public void SetControlledOffensive()
    {
        controlledOffensive = null;
    }

    public void SetIsUsingOffensive(bool val)
    {
        isUsingOffensive = val;
    }

    public bool GetIsUsingOffensive()
    {
        return isUsingOffensive;
    }

    #endregion
}
