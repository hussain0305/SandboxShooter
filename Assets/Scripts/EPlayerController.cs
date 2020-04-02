using Photon.Pun;
using System.IO;
using UnityEngine;

//RUNNING : Player can select between toggling and holding the run button

public class EPlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerUI playerUI;
    public PlayerEnergy playerEnergy;
    public PlayerDisbalance playerDisbalance;
    public EPlayerMovement playerMovement;
    public PlayerWeapons handledWeapons;

    [Header("Game Settings")]
    public float mouseSensitivity = 50;

    [Header("Camera")]
    public Camera playerCamera;
    public GameObject cameraHolder;

    [Header("Misc")]
    public ParticleSystem deathEffect;

    const float BUILD_DISTANCE = 20; 
    [HideInInspector]
    public bool constructionMenuOpen;
    [HideInInspector]
    public bool anyMenuOpen;
    
    
    private bool isUsingOffensive;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private Vector3 camPosition;
    private Quaternion camRotation;
    private Transform playerBody;
    private Animator characterAnimator;
    private GameManager gameManager;
    private CharacterController characterController;
    private OffensiveControllerBase offsensiveInVicinity;
    private EPlayerNetworkPresence networkPresence;
    private int networkID;

    private PhotonView pView;

    void Awake()
    {
        FetchComponents();
        InitializeValues();

        GetPlayerPreferences();
    }

    void FetchComponents()
    {
        playerBody = GetComponent<Transform>();
        playerMovement = GetComponent<EPlayerMovement>();
        handledWeapons = GetComponent<PlayerWeapons>();
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        //playerCamera = Camera.main;
        pView = GetComponent<PhotonView>();

        if (!pView.IsMine)
        {
            return;
        }

        playerUI.enabled = true;
        playerEnergy.enabled = true;
        playerCamera.gameObject.SetActive(true);
    }

    void InitializeValues()
    {
        camPosition = playerCamera.transform.localPosition;
        camRotation = playerCamera.transform.localRotation;

        if (!pView.IsMine)
        {
            return;
        }

        constructionMenuOpen = false;
        anyMenuOpen = false;

        SetIsUsingOffensive(false);

        //characterController.detectCollisions = false;
    }

    void GetPlayerPreferences()
    {
        mouseSensitivity = 4 * PlayerPrefs.GetInt("MouseSensitivity", 50);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pView.IsMine || !characterAnimator || !characterController)
        {
            return;
        }
        //No Need to proceed if critical components aren't established isn't established

        if (Input.GetButtonDown("PauseMenu"))
        {
            anyMenuOpen = playerUI.TogglePauseMenu();
        }

        if (anyMenuOpen)
        {
            return;
        }
        //If pause menu is open, no other inputs need to be processed

        if (Input.GetButtonDown("Use"))
        {
            if (offsensiveInVicinity && !isUsingOffensive && !constructionMenuOpen && !handledWeapons.isADSing)
            {
                OccupyOffensive();
            }

            else if (offsensiveInVicinity && isUsingOffensive)
            {
                LeaveOffensive();
            }
        }

        if (isUsingOffensive)
        {
            return;
        }

        if (Input.GetButtonDown("Build") && !isUsingOffensive)
        {
            if (constructionMenuOpen)
            {
                ToggleConstructionMenu();
            }
            else
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
        }

        if (constructionMenuOpen)
        {
            return;
        }
        //No turning, looking around or accessing Scoreboard and quick build menu if construction menu is open
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            playerUI.QuickMenuScroll(Input.GetAxis("Mouse ScrollWheel"));
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (playerEnergy.hasEnergyPack)
            {
                playerUI.QuickConstruct();
            }
            else
            {
                playerUI.DisplayAlertMessage("Cannot construct without energy pack");
            }
        }

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 35f);
        cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetButtonDown("Scoreboard"))
        {
            playerUI.SetScoreboardActive(true);
        }

        else if (Input.GetButtonUp("Scoreboard"))
        {
            playerUI.SetScoreboardActive(false);
        }
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
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, BUILD_DISTANCE,
            ~(1 << LayerMask.NameToLayer("ProximitySensor"))))
        {
            if (hit.collider.tag == "GridFloor" || hit.collider.tag == "GridFlying")
            {
                return true;
            }
        }
        return false;
    }

    public bool GetSpawnLocationAndRotation(out Vector3 location, out Quaternion rotation)
    {
        Vector3 loc = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        location = loc;
        rotation = rot;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, BUILD_DISTANCE,
            ~(1 << LayerMask.NameToLayer("ProximitySensor"))))
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
            
            else if (hit.collider.tag.Equals("GridFlying"))
            {
                if (AlignOnFlyingGrid(hit, out location))
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

    bool AlignOnFlyingGrid(RaycastHit hit, out Vector3 location)
    {
        Vector3 localGridCoords = hit.transform.InverseTransformPoint(hit.point);
        Vector3 localAlignedCoords = new Vector3(
            (localGridCoords.x > (0.5f / 3) ? 0.33f : (localGridCoords.x < (-0.5f / 3)) ? -0.33f : 0),
            localGridCoords.y,
            (localGridCoords.z > (0.5f / 3) ? 0.33f : (localGridCoords.z < (-0.5f / 3)) ? -0.33f : 0));

        location = hit.transform.TransformPoint(localAlignedCoords);

        return !gameManager.ObjectExistsAtLocation(location);
    }
    #endregion

    void OccupyOffensive()
    {
        SetIsUsingOffensive(true);
        offsensiveInVicinity.OffensiveOccuppied(this);        
        playerUI.RemoveInstructionMessage();

        playerMovement.StopInherentMovement();
        transform.position = offsensiveInVicinity.playerAnchor.transform.position;
        playerCamera.transform.SetParent(offsensiveInVicinity.turret);
        playerCamera.transform.localPosition = offsensiveInVicinity.cameraAnchor.transform.localPosition;
        playerCamera.transform.localRotation = offsensiveInVicinity.cameraAnchor.transform.localRotation;
    }

    public void LeaveOffensive()
    {
        if (!pView.IsMine)
        {
            return;
        }
        SetIsUsingOffensive(false);
        playerCamera.transform.SetParent(cameraHolder.transform);
        playerCamera.transform.localPosition = camPosition;
        playerCamera.transform.localRotation = camRotation;
        offsensiveInVicinity.OffensiveLeft();
        SetOffensiveInVicinity();
    }

    public bool IsInActiveGameplay()
    {
        //function to see whether the player is in active gameplay or
        //has any UI/menu open. keep adding to the condition as more
        //UI artifacts are added.

        return !(constructionMenuOpen || isUsingOffensive || anyMenuOpen);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pView.IsMine)
        {
            return;
        }
    }

    public void BodyPartHit(float disbalanceImpact, int attacker)
    {
        if (playerEnergy.hasEnergyPack)
        {
            playerDisbalance.AddOnDisbalance(disbalanceImpact);
        }
        else
        {
            //health deducation here, not straight death
            PlayerIsDead(attacker);
        }
    }

    public bool IsLocalPView()
    {
        return pView.IsMine;
    }

    public void PickedUpEnergyWeapon(EnergyWeaponBase pickedWeapon)
    {
        playerEnergy.energyPack.SetEnergyWeapon(PhotonNetwork.Instantiate(Path.Combine(pickedWeapon.pathStrings),
            Vector3.zero, Quaternion.identity).GetComponent<EnergyWeaponBase>());
    }

    public void PickedUpHandledWeapon(HandledWeaponBase pickedWeapon)
    {
        handledWeapons.PickUpWeapon(PhotonNetwork.Instantiate(Path.Combine(pickedWeapon.pathStrings),
            Vector3.zero, Quaternion.identity).GetComponent<HandledWeaponBase>());
    }


    public void PlayerIsDead(int attacker)
    {
        if (isUsingOffensive && offsensiveInVicinity)
        {
            offsensiveInVicinity.ForceEjectPlayer();
        }
        playerUI.ShowRespawnScreen(gameManager.GetRespawnDuration());
        playerCamera.transform.parent = null;
        playerCamera.GetComponent<CameraDestroyer>().DestroyIn(gameManager.GetRespawnDuration());
        networkPresence.StartRespawnCooldown(gameManager.GetRespawnDuration());
        DestroyPlayerModel(networkID, attacker);//CommunicateDeathToClients
    }

    public void DestroyPlayerModel(int deceased, int attacker)
    {
        if (pView.IsMine)
        {
            pView.RPC("RPC_CommunicateDeathToClients", RpcTarget.All, deceased, attacker);
            PhotonNetwork.Destroy(this.gameObject);

        }
    }

    [PunRPC]
    void RPC_CommunicateDeathToClients(int deceased, int attacker)
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
        if (attacker != 0 && PhotonView.Find(attacker))
        {
            PhotonView.Find(attacker).GetComponent<EPlayerNetworkPresence>().KilledPlayer();
        }
        if (PhotonView.Find(deceased))
        {
            PhotonView.Find(deceased).GetComponent<EPlayerNetworkPresence>().WasKilled();
        }
    }


    #region Getters and Setters

    public OffensiveControllerBase GetControlledOffensive()
    {
        return offsensiveInVicinity;
    }

    public void SetOffensiveInVicinity(OffensiveControllerBase controllee)
    {
        offsensiveInVicinity = controllee;
    }

    public void SetOffensiveInVicinity()
    {
        offsensiveInVicinity = null;
    }

    public void SetIsUsingOffensive(bool val)
    {
        isUsingOffensive = val;
        playerMovement.SetIsUsingOffensive(val);
    }

    public bool GetIsUsingOffensive()
    {
        return isUsingOffensive;
    }

    public void SetNetworkPresence(EPlayerNetworkPresence presence, int id)
    {
        networkPresence = presence;
        //networkID = id;
        pView.RPC("RPC_SetNetworkIDOnClients", RpcTarget.All, id);
    }

    [PunRPC]
    void RPC_SetNetworkIDOnClients(int id)
    {
        networkID = id;
    }

    public int GetNetworkID()
    {
        return networkID;
    }

    public EPlayerNetworkPresence GetNetworkPresence()
    {
        return networkPresence;
    }

    #endregion

}
