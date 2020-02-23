using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum Direction { Forward, Backward, Right, Left, Up, Down };

public class AutoNavFlyer : MonoBehaviour
{

    private PhotonView pView;
    private Rigidbody rBody;

    private readonly float flySpeed = 7.5f;

    private List<Direction> availableDirections;

    private PhotonTransformViewClassic pTransform;

    void Start()
    {
        pView = GetComponent<PhotonView>();
        rBody = GetComponent<Rigidbody>();
        pTransform = GetComponent<PhotonTransformViewClassic>();

        availableDirections = new List<Direction> { Direction.Up, Direction.Down,
            Direction.Forward, Direction.Backward, Direction.Right, Direction.Left };

    }

    public bool PViewIsMine()
    {
        return pView.IsMine;
    }

    public void StartMoving()
    {
        if (!pView.IsMine)
        {
            return;
        }

        StartCoroutine(FireUp());
    }

    IEnumerator FireUp()
    {
        yield return new WaitForSeconds(1);
        MoveInDirection(Direction.Up);
        StartCoroutine(LookForRedirection());
    }

    public void StopMoving()
    {
        rBody.velocity = Vector3.zero;
        Redirect();
    }

    public void MoveInDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                rBody.velocity = transform.up * flySpeed; ;
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

        pTransform.SetSynchronizedValues(rBody.velocity, 0);
    }

    public void Redirect()
    {
        MoveInDirection(availableDirections[Random.Range(0, availableDirections.Count)]);
    }

    public void DirectionUnavailable(Direction blockDir)
    {
        availableDirections.Remove(blockDir);
    }

    public void DirectionAvailable(Direction availDir)
    {
        if (availableDirections.Contains(availDir))
        {
            return;
        }
        availableDirections.Add(availDir);
    }

    IEnumerator LookForRedirection()
    {
        while (true)
        {
            Redirect();
            yield return new WaitForSeconds(5);
        }
    }

}