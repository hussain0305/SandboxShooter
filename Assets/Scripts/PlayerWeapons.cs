using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject spine;
    public GameObject cameraHolder;
    public GameObject cam;

    [HideInInspector]
    public bool hasWeapon = false;
    [HideInInspector]
    public bool isADSing = false;
    [HideInInspector]
    public HandledWeaponBase currentWeapon;

    private Animator playerAnimator;
    private Animator camAnimator;
    private EPlayerController player;

    private float mouseY;
    private float xRotation;
    private float adsSenstivity = 200;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        camAnimator = cam.GetComponent<Animator>();
        player = GetComponent<EPlayerController>();
        playerAnimator.SetLayerWeight(0, 1f);
        playerAnimator.SetLayerWeight(1, 0f);
    }

    private void Update()
    {
        if (!hasWeapon)
        {
            return;
        }

        if (Input.GetButtonDown("Fire2") && !player.GetIsUsingOffensive())
        {
            StartADS();
        }

        if (Input.GetButtonUp("Fire2") && !player.GetIsUsingOffensive())
        {
            StopADS();
        }

        if (Input.GetButtonDown("ThrowWeapon"))
        {
            ThrowWeapon();
        }
    }

    private void LateUpdate()
    {
        if (!hasWeapon || !isADSing)
        {
            return;
        }
        if (isADSing)
        {
            mouseY = Input.GetAxis("Mouse Y") * adsSenstivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45f, 35f);
            spine.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        }
    }

    public void PickUpWeapon(HandledWeaponBase pickedWeapon)
    {
        currentWeapon = pickedWeapon;
        pickedWeapon.transform.SetParent(weaponHolder);
        pickedWeapon.transform.localPosition = Vector3.zero;
        pickedWeapon.transform.localRotation = Quaternion.identity;

        hasWeapon = true;
        playerAnimator.SetLayerWeight(1, 1f);
    }

    public void ThrowWeapon()
    {
        if (isADSing)
        {
            return;
        }

        currentWeapon = null;
        hasWeapon = false;
        playerAnimator.SetLayerWeight(1, 0f);
    }

    public void DropWeapon()
    {
        if (isADSing)
        {
            return;
        }
        currentWeapon = null;
        hasWeapon = false;
        playerAnimator.SetLayerWeight(1, 0f);

    }

    public void StartADS()
    {
        playerAnimator.SetTrigger("goADS");
        camAnimator.enabled = false;
        isADSing = true;
        player.playerMovement.SetADSspeed(isADSing);
        StopAllCoroutines();
        StartCoroutine(MoveCamera(currentWeapon.GetADSPosition(), true));
    }

    public void StopADS()
    {
        playerAnimator.SetTrigger("goHip");
        camAnimator.enabled = true;
        isADSing = false;
        player.playerMovement.SetADSspeed(isADSing);
        StopAllCoroutines();
        StartCoroutine(MoveCamera(cameraHolder, false));
    }

    IEnumerator MoveCamera(GameObject targetPosition, bool isOnWeapon)
    {
        while(Vector3.Distance(cam.transform.position, targetPosition.transform.position) > 0.05f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition.transform.position, 10 * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetPosition.transform.rotation, 10 * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        cam.transform.SetPositionAndRotation(targetPosition.transform.position, targetPosition.transform.rotation);

        if (isOnWeapon)
        {
            cam.transform.SetParent(targetPosition.transform.parent);
        }
        else
        {
            cam.transform.SetParent(targetPosition.transform);
        }
    }
}
