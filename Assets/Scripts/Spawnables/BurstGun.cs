using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class BurstGun : OffensiveControllerBase
{
    private Projectile proj;
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!isOccupied)
        {
            return;
        }
        if (controllerPView.IsMine)
        {
            mouseX = Input.GetAxis("Mouse X") * turretFluidity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * turretFluidity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -75f, 75f);
            //turret.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            pView.RPC("RPC_ClientControlledRotation", RpcTarget.All, xRotation, yRotation);
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                Shoot();
            }
        }
    }

    [PunRPC]
    void RPC_ClientControlledRotation(float x, float y)
    {
        turret.localRotation = Quaternion.Euler(x, y, 0);
    }

    public void Shoot()
    {
        Shot();
        StartCoroutine(BurstShot());
    }

    IEnumerator BurstShot()
    {
        for (int loop = 0; loop < 3; loop++)
        {
            //proj = PhotonNetwork.Instantiate(Path.Combine(projectile.pathStrings), nozzle.transform.position, nozzle.transform.rotation, 0).GetComponent<Projectile>();
            pView.RPC("RPC_SpawnCannonball", RpcTarget.All, controllingPlayer.GetNetworkID());
            yield return new WaitForSeconds(0.1f);
        }
    }

    [PunRPC]
    void RPC_SpawnCannonball(int id)
    {
        proj = Instantiate(projectile, nozzle.transform.position, nozzle.transform.rotation);
        SetDamageOnProjectile(proj);
        proj.GetComponent<Rigidbody>().AddForce(nozzle.transform.forward * projectForce);
        proj.SetOwnerID(id);
    }
}
