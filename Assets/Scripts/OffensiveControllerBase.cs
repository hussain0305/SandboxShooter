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

    protected bool isOccupied;
    private Quaternion originalOrientation;
    private SpawnableGO master;
    private int controllingPlayer;

    protected PhotonView pView;
    //private EPlayerController controllingPlayer;

    public void Start()
    {
        canShoot = true;
        originalOrientation = transform.rotation;
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
            other.gameObject.GetComponent<EPlayerController>().SetControlledOffensive(this);
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
            other.gameObject.GetComponent<EPlayerController>().SetControlledOffensive();
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
        controllingPlayer = controller;
        isOccupied = true;
        PhotonView.Find(controller).transform.position = playerAnchor.transform.position;
        Camera cam = PhotonView.Find(controller).GetComponentInChildren<Camera>();
        cam.transform.SetParent(turret);
        cam.transform.localPosition = cameraAnchor.transform.localPosition;
        cam.transform.localRotation = cameraAnchor.transform.localRotation;
    }

    public void OffensiveLeft()
    {
        pView.RPC("RPC_OffensiveLeft", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_OffensiveLeft()
    {
        Debug.Log(controllingPlayer + " left " + name);
        controllingPlayer = -1;
        isOccupied = false;
        transform.rotation = originalOrientation;
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

    #region Getters and Setters
    private void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    private bool GetIsOccupied()
    {
        return isOccupied;
    }

    private void SetControllingPlayer(int controller)
    {
        controllingPlayer = controller;
    }

    private int GetControllingPlayer()
    {
        return controllingPlayer;
    }
    #endregion
}
