using System.Collections;
using System.Collections.Generic;
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

    protected bool canShoot;
    protected float mouseX;
    protected float mouseY;
    protected float xRotation;
    protected float yRotation;

    protected bool isOccupied;
    private Quaternion originalOrientation;
    private SpawnableGO master;
    private EPlayerController controllingPlayer;

    public void Start()
    {
        canShoot = true;
        originalOrientation = transform.rotation;
        master = GetComponentInParent<SpawnableGO>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isOccupied || !master.isUsable)
        {
            return;
        }

        if (other.gameObject.GetComponent<EPlayerController>())
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
        if (other.gameObject.GetComponent<EPlayerController>())
        {
            other.gameObject.GetComponent<EPlayerController>().playerUI.RemoveInstructionMessage();
            other.gameObject.GetComponent<EPlayerController>().SetControlledOffensive();
        }
    }

    public void OffensiveOccuppied(EPlayerController controller, Camera cam)
    {
        controllingPlayer = controller;
        isOccupied = true;
        controllingPlayer.transform.position = playerAnchor.transform.position;
        cam.transform.SetParent(this.transform);
        cam.transform.localPosition = cameraAnchor.transform.localPosition;
        cam.transform.localRotation = cameraAnchor.transform.localRotation;
    }

    public void OffensiveLeft()
    {
        controllingPlayer = null;
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

    private void SetControllingPlayer(EPlayerController controller)
    {
        controllingPlayer = controller;
    }

    private EPlayerController GetControllingPlayer()
    {
        return controllingPlayer;
    }
    #endregion
}
