using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : OffensiveControllerBase
{
    private Projectile proj;
    new void Start()
    {
        base.Start();
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
                Shoot();
            }
        }
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
            proj = Instantiate(projectile, nozzle.transform.position, nozzle.transform.rotation);
            SetDamageOnProjectile(proj);
            proj.GetComponent<Rigidbody>().AddForce(nozzle.transform.forward * projectForce);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
