using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpedMovement : MonoBehaviour
{
    public float nextY;

    private Vector3 toPosition;

    private float startingAngle;
    private float waveFactor;

    public void GoToPosition(Vector3 pos)
    {
        toPosition = pos;
        StartCoroutine(LerpPosition());
    }

    public void SetStartingAngle(float val, float wave)
    {
        startingAngle = val;
        waveFactor = wave;
        StartCoroutine(SelfMovement());

    }
    IEnumerator LerpPosition()
    {
        while(Vector3.Distance(transform.localPosition, toPosition) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, toPosition, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = toPosition;
    }

    IEnumerator SelfMovement()
    {
        float simplifiedAngle = startingAngle;
        while (true)
        {
            simplifiedAngle = (simplifiedAngle + waveFactor) % 360;
            nextY = 10 * Mathf.Sin(simplifiedAngle);
            toPosition = new Vector3(transform.localPosition.x, nextY, transform.localPosition.z);
            StopAllCoroutines();
            StartCoroutine(LerpPosition());
            yield return new WaitForSeconds(1);
        }
    }
}
