using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Doesn't really need any inspector input, so I added this script in Player Movement 
//for cleaner organization. Separating the external movement allows to not unnecessarily
//call the LateUpdate function unnecessarily every frame even when the player isn't on it
//as would've been the case had this been PlayerMovement

public class PlayerExternalMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection;

    private void Awake()
    {
        SetupEverything();
    }

    void SetupEverything()
    {
        characterController = GetComponent<CharacterController>();
        moveDirection = new Vector3(0, 0, 0);
    }

    public void LateUpdate()
    {
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void AddToVelocity(Vector3 direction)
    {
        moveDirection = direction;
    }

}
