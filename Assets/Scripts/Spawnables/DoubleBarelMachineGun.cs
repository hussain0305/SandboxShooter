using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class DoubleBarelMachineGun : OffensiveControllerBase
{
    const float BULLET_OFFSET = 0.5f;

    private bool shooting;
    private Projectile proj;
    new void Start()
    {
        base.Start();
        shooting = false;
    }

    // Update is called once per frame
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
            yRotation = Mathf.Clamp(yRotation, -75, 75);
            //turret.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            pView.RPC("RPC_ClientControlledRotation", RpcTarget.All, xRotation, yRotation);
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                shooting = true;
                Shoot();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                shooting = false;
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
        //Shot();
        StartCoroutine(MachineGunShot());
    }

    IEnumerator MachineGunShot()
    {
        while(shooting)
        {
            if (controllingPlayer)
            {
                pView.RPC("RPC_SpawnBullet", RpcTarget.All, nozzle.transform.position - (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation, controllingPlayer.GetNetworkID());

                pView.RPC("RPC_SpawnBullet", RpcTarget.All, nozzle.transform.position + (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation, controllingPlayer.GetNetworkID());
            }
            else
            {
                pView.RPC("RPC_SpawnBulletWithoutOwner", RpcTarget.All, nozzle.transform.position - (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation);

                pView.RPC("RPC_SpawnBulletWithoutOwner", RpcTarget.All, nozzle.transform.position + (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation);
            }

            yield return new WaitForSeconds(cooldown);
        }
    }

    [PunRPC]
    void RPC_SpawnBullet(Vector3 pos, Quaternion rot, int id)
    {
        proj = Instantiate(projectile, pos, rot).GetComponent<Projectile>();
        proj.GetComponent<Rigidbody>().velocity = proj.transform.forward * projectForce;
        SetDamageOnProjectile(proj);

        proj.SetOwnerID(id);
    }

    [PunRPC]
    void RPC_SpawnBulletWithoutOwner(Vector3 pos, Quaternion rot)
    {
        proj = Instantiate(projectile, pos, rot).GetComponent<Projectile>();
        proj.GetComponent<Rigidbody>().velocity = proj.transform.forward * projectForce;
        SetDamageOnProjectile(proj);
    }

}
