using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine;

public class DoubleBarelMachineGun : OffensiveControllerBase
{
    const float BULLET_OFFSET = 0.25f;
    const float RECOIL_RANGE = 0.25f;

    private bool shooting;
    private Projectile proj;

    private TurretRecoiler recoiler;
    private Vector2 recoilFactor;

    new void Start()
    {
        base.Start();
        recoilFactor = Vector2.zero;
        shooting = false;
        recoiler = GetComponentInChildren<TurretRecoiler>();
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
            xRotation -= recoilFactor.x;
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -75, 75);
            yRotation -= recoilFactor.y * (Random.Range(1, 10) < 5 ? -1 : 1);
            pView.RPC("RPC_ClientControlledRotation", RpcTarget.All, xRotation, yRotation);
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                pView.RPC("RPC_SetShooting", RpcTarget.All, 1);
                Shoot();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                pView.RPC("RPC_SetShooting", RpcTarget.All, 0);
            }
        }
    }

    [PunRPC]
    void RPC_SetShooting(int boolInt)
    {
        shooting = true;
        recoilFactor = new Vector2(0.1f, 0.1f);
        
        if (boolInt == 0)
        {
            shooting = false;
            recoilFactor = new Vector2(0, 0);
        }
    }

    [PunRPC]
    void RPC_ClientControlledRotation(float x, float y)
    {
        turret.localRotation = Quaternion.Euler(x, y, 0);
    }

    public void Shoot()
    {
        StartCoroutine(MachineGunShot());
    }

    IEnumerator MachineGunShot()
    {
        while (shooting)
        {
            if (controllingPlayer)
            {
                pView.RPC("RPC_SpawnBullet", RpcTarget.All, 
                    nozzle.transform.position - (BULLET_OFFSET * nozzle.transform.right) + new Vector3(Random.Range(-RECOIL_RANGE, RECOIL_RANGE), Random.Range(-RECOIL_RANGE, RECOIL_RANGE), 0), 
                    nozzle.transform.rotation, controllingPlayer.GetNetworkID());

                pView.RPC("RPC_SpawnBullet", RpcTarget.All, 
                    nozzle.transform.position + (BULLET_OFFSET * nozzle.transform.right) + new Vector3(Random.Range(-RECOIL_RANGE, RECOIL_RANGE), Random.Range(-RECOIL_RANGE, RECOIL_RANGE), 0), 
                    nozzle.transform.rotation, controllingPlayer.GetNetworkID());
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

        recoiler.Recoil();
    }

    [PunRPC]
    void RPC_SpawnBulletWithoutOwner(Vector3 pos, Quaternion rot)
    {
        proj = Instantiate(projectile, pos, rot).GetComponent<Projectile>();
        proj.GetComponent<Rigidbody>().velocity = proj.transform.forward * projectForce;
        SetDamageOnProjectile(proj);
        
        recoiler.Recoil();
    }
}
