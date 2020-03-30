using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [HideInInspector]
    public bool hasWeapon = false;

    [HideInInspector]
    public HandledWeaponBase currentWeapon;

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetLayerWeight(0, 1f);
        playerAnimator.SetLayerWeight(1, 0f);
    }

    private void Update()
    {
        if (!hasWeapon)
        {
            return;
        }
        if (Input.GetButtonDown("ThrowWeapon"))
        {
            ThrowWeapon();
        }
    }

    public void PickUpWeapon(HandledWeaponBase pickedWeapon)
    {
        hasWeapon = true;
        Debug.Log("Picked up " + pickedWeapon.name);
        playerAnimator.SetLayerWeight(1, 1f);
    }

    public void ThrowWeapon()
    {
        hasWeapon = false;
        playerAnimator.SetLayerWeight(1, 0f);
    }

    public void DropWeapon()
    {

    }
}
