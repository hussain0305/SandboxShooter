using Photon.Pun;
using System.Collections;
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
        moveDirection = Vector3.zero;
    }

    public void LateUpdate()
    {
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void SetVelocity(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckIfStillOnGround());
    }

    IEnumerator CheckIfStillOnGround()
    {
        while (true)
        {
            if(Physics.CheckSphere(transform.position, 4, (1 << LayerMask.NameToLayer("BuildingDetector")))){

            }
            else
            {
                ForceStop();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void ForceStop()
    {
        StopAllCoroutines();
        this.enabled = false;
    }

}
