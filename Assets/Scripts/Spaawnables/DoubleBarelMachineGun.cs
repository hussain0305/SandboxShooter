using System.Collections;
using System.Collections.Generic;
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
        if (isOccupied)
        {
            mouseX = Input.GetAxis("Mouse X") * turretFluidity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * turretFluidity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 15f);
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -75f, 75f);
            this.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
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

    public void Shoot()
    {
        //Shot();
        StartCoroutine(MachineGunShot());
    }

    IEnumerator MachineGunShot()
    {
        while(shooting)
        {
            proj = Instantiate(projectile, nozzle.transform.position - (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation);
            proj.GetComponent<Rigidbody>().AddForce(nozzle.transform.forward * projectForce);
            SetDamageOnProjectile(proj);
            proj = Instantiate(projectile, nozzle.transform.position + (BULLET_OFFSET * nozzle.transform.right), nozzle.transform.rotation);
            proj.GetComponent<Rigidbody>().AddForce(nozzle.transform.forward * projectForce);
            SetDamageOnProjectile(proj);

            yield return new WaitForSeconds(0.05f);
        }
    }
}
