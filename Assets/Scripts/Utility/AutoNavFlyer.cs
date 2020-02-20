using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum Direction { Forward, Backward, Right, Left, Up, Down };

public class AutoNavFlyer : MonoBehaviour
{
    public Transform probeForward;
    public Transform probeBackward;
    public Transform probeRight;
    public Transform probeLeft;
    public Transform probeUp;
    public Transform probeDown;

    private Direction currentDirection;

    private PhotonView pView;
    private Rigidbody rBody;

    private readonly float flySpeed = 7.5f;

    private Direction[] allDirections;

    void Start()
    {
        pView = GetComponent<PhotonView>();
        rBody = GetComponent<Rigidbody>();

        allDirections = new Direction[] { Direction.Up, Direction.Down,
            Direction.Forward, Direction.Backward, Direction.Right, Direction.Left };

        if (!pView.IsMine)
        {
            return;
        }

        MoveInDirection(Direction.Up);

        StartCoroutine(CheckForImminentCollisions());
    }

    IEnumerator CheckForImminentCollisions()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            if (CollisionIsImminent(currentDirection) || UnityEngine.Random.Range(1, 20) > 15)
            {
                //Debug.Log("Changing Direction");
                ChangeDirection();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void MoveInDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                rBody.velocity = transform.up * flySpeed;
                break;
            case Direction.Down:
                rBody.velocity = -1 * transform.up * flySpeed;
                break;
            case Direction.Forward:
                rBody.velocity = transform.forward * flySpeed;
                break;
            case Direction.Backward:
                rBody.velocity = -1 * transform.forward * flySpeed;
                break;
            case Direction.Right:
                rBody.velocity = transform.right * flySpeed;
                break;
            case Direction.Left:
                rBody.velocity = -1 * transform.right * flySpeed;
                break;
        }

        currentDirection = direction;
    }

    bool CollisionIsImminent(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                if (Physics.CheckSphere(probeUp.position, 2))
                {
                    //Debug.Log("Up Collision Imminent");
                    return true;
                }
                break;
            case Direction.Down:
                if (Physics.CheckSphere(probeDown.position, 2))
                {
                    //Debug.Log("Down Collision Imminent");
                    return true;
                }
                break;
            case Direction.Forward:
                if (Physics.CheckSphere(probeForward.position, 2))
                {
                    //Debug.Log("Fwd Collision Imminent");
                    return true;
                }
                break;
            case Direction.Backward:
                if (Physics.CheckSphere(probeBackward.position, 2))
                {
                    //Debug.Log("Bwd Collision Imminent");
                    return true;
                }
                break;
            case Direction.Left:
                if (Physics.CheckSphere(probeLeft.position, 2))
                {
                    //Debug.Log("Left Collision Imminent");
                    return true;
                }
                break;
            case Direction.Right:
                if (Physics.CheckSphere(probeRight.position, 2))
                {
                    //Debug.Log("Right Collision Imminent");
                    return true;
                }
                break;
        }

        return false;

    }

    public void ChangeDirection()
    {
        int startAt = UnityEngine.Random.Range(0, allDirections.Length);
        int curr;
        for (int loop = 0; loop < allDirections.Length; loop++)
        {
            curr = (startAt + loop) % allDirections.Length;
            if (!CollisionIsImminent(allDirections[curr]))
            {
                //Debug.Log("Decided to go " + allDirections[curr]);

                MoveInDirection(allDirections[curr]);
                break;
            }
        }
    }
}