using Photon.Pun;
using System.Collections;
using UnityEngine;

public class OffensiveControllerBase : MonoBehaviour
{
    public GameObject playerAnchor;
    public GameObject cameraAnchor;

    [Header("Turret Settings")]
    public GameObject nozzle;
    public float turretFluidity;
    public Projectile projectile;
    public int damage;
    public float projectForce;
    public float cooldown;
    public Transform turret;

    protected bool canShoot;
    protected float mouseX;
    protected float mouseY;
    protected float xRotation;
    protected float yRotation;

    public bool isOccupied;//protected
    private Quaternion originalOrientation;
    private SpawnableGO master;

    protected EPlayerController controllingPlayer;
    protected PhotonView pView;
    protected PhotonView controllerPView;

    public void Start()
    {
        canShoot = true;
        originalOrientation = turret.transform.rotation;
        master = GetComponentInParent<SpawnableGO>();
        pView = GetComponent<PhotonView>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isOccupied || !master.isUsable)
        {
            return;
        }

        if (other.gameObject.GetComponent<EPlayerController>() && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.gameObject.GetComponent<EPlayerController>().playerUI.DisplayInstructionMessage("Press E to use " + master.displayName);
            other.gameObject.GetComponent<EPlayerController>().SetOffensiveInVicinity(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isOccupied)
        {
            return;
        }
        if (other.gameObject.GetComponent<EPlayerController>() && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.gameObject.GetComponent<EPlayerController>().playerUI.RemoveInstructionMessage();
            other.gameObject.GetComponent<EPlayerController>().SetOffensiveInVicinity();
        }
    }

    public void OffensiveOccuppied(EPlayerController controller)
    {
        int tID = controller.GetComponent<PhotonView>().ViewID;
        pView.RPC("RPC_OffensiveOccupied", RpcTarget.All, tID);
    }

    [PunRPC]
    public void RPC_OffensiveOccupied(int controller)
    {
        controllingPlayer = PhotonView.Find(controller).GetComponent<EPlayerController>();
        controllerPView = PhotonView.Find(controller).GetComponent<PhotonView>();
        isOccupied = true;
    }

    public void OffensiveLeft()
    {
        pView.RPC("RPC_OffensiveLeft", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_OffensiveLeft()
    {
        controllingPlayer = null;
        controllerPView = null;
        isOccupied = false;
        turret.transform.rotation = originalOrientation;
    }

    public void Shot()
    {
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    public void SetDamageOnProjectile(Projectile proj)
    {
        proj.SetDamage(damage);
    }

    public void ForceEjectPlayer()
    {
        if (controllingPlayer)
        {
            controllingPlayer.LeaveOffensive();
        }
    }

    #region Getters and Setters
    public void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    public bool GetIsOccupied()
    {
        return isOccupied;
    }

    public void SetControllingPlayer(EPlayerController controller)
    {
        controllingPlayer = controller;
    }

    #endregion
}
