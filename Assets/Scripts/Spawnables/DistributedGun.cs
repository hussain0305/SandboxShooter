using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DistributedGun : MonoBehaviour
{
    const float DISTANCE = 2000;
    const float TURN_TIME = 0.1f;
    const int DAMAGE = 30;
    const float BULLET_SPEED = 1200;
    public Projectile projectile;
    public GameObject rail;
    public GameObject nozzle;
    public GameObject nozzleHead;    
    
    private PhotonView pView;
    private RaycastHit hit;
    private Camera ownerCurrentViewport;
    private float elapsedTime;
    private Quaternion startingRotation;
    private Quaternion targetRotation;
    private Vector3 railAimDirection;
    private Projectile spawnedProjectile;
    private EPlayerController master;

    void Start()
    {
        pView = GetComponent<PhotonView>();

        if (!pView.IsMine)
        {
            return;
        }
        
        if (ownerCurrentViewport)
        {
            StartCoroutine(SeeIfFiring());
        }
    }

    IEnumerator SeeIfFiring()
    {
        while (true)
        {
            if (Input.GetButton("Fire1"))
            {
                if(Physics.Raycast(ownerCurrentViewport.transform.position, ownerCurrentViewport.transform.forward, out hit, DISTANCE))
                {
                    pView.RPC("RPC_ShootAt", RpcTarget.All, hit.point, master.GetNetworkID());
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    [PunRPC]
    void RPC_ShootAt(Vector3 hitPoint, int iD)
    {
        StartCoroutine(RotateRail(hitPoint, iD));
    }

    public void SetOwner(EPlayerController owner)
    {
        master = owner;
        ownerCurrentViewport = owner.playerCamera;
    }

    IEnumerator RotateRail(Vector3 hitPoint, int ownerID)
    {
        railAimDirection = hitPoint - rail.transform.position;
        railAimDirection.y = rail.transform.position.y;

        startingRotation = rail.transform.rotation;
        targetRotation = Quaternion.LookRotation(railAimDirection.normalized);
        elapsedTime = 0;

        //First rotate the rail

        while (elapsedTime < TURN_TIME)
        {
            elapsedTime += Time.deltaTime;

            rail.transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / TURN_TIME));
            yield return new WaitForEndOfFrame();
        }

        //Then rotate the nozzle

        startingRotation = nozzle.transform.rotation;
        targetRotation = Quaternion.LookRotation((hitPoint - nozzle.transform.position).normalized);
        elapsedTime = 0;
        
        while (elapsedTime < TURN_TIME)
        {
            elapsedTime += Time.deltaTime;

            nozzle.transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / TURN_TIME));
            yield return new WaitForEndOfFrame();
        }

        spawnedProjectile = Instantiate(projectile, nozzleHead.transform.position, nozzleHead.transform.rotation).GetComponent<Projectile>();
        spawnedProjectile.SetDamage(DAMAGE);
        spawnedProjectile.GetComponent<Rigidbody>().AddForce(nozzle.transform.forward * BULLET_SPEED);
        spawnedProjectile.SetOwnerID(ownerID);
    }

}