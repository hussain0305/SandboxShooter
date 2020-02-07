using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLift : MonoBehaviour
{
    public Vector3 downPosition;
    public Vector3 upPosition;
    public float liftSpeed;

    private Vector3 destination;
    private bool isMoving;

    void Start()
    {
        isMoving = false;
        transform.localPosition = downPosition;
    }

    IEnumerator MoveLift()
    {
        isMoving = true;
        GetComponent<Rigidbody>().velocity = transform.up * liftSpeed *
            ((transform.localPosition.y < destination.y) ? 1 : -1);
        while (Vector3.Distance(transform.localPosition, destination) > 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localPosition = destination;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EPlayerController>())
        {
            destination = upPosition;
            StopAllCoroutines();
            StartCoroutine(MoveLift());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<EPlayerController>())
        {
            destination = downPosition;
            StopAllCoroutines();
            StartCoroutine(MoveLift());
        }
    }
}