using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    float xRotation, yRotation;
    public void DestroyIn(float seconds)
    {
        GetComponent<Animator>().enabled = false;
        transform.SetPositionAndRotation(GameObject.FindObjectOfType<RespawnCameraPosition>().transform.position,
            GameObject.FindObjectOfType<RespawnCameraPosition>().transform.rotation);
        StartCoroutine(Countdown(seconds));
    }

    IEnumerator Countdown(float duration)
    {
        StartCoroutine(EnableCameraLook());
        yield return new WaitForSeconds(duration);
        StopCoroutinesAndDestroy();
    }

    public void StopCoroutinesAndDestroy()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    IEnumerator EnableCameraLook()
    {
        while (true)
        {
            xRotation -= Input.GetAxis("Mouse Y") * 300 * Time.deltaTime;
            yRotation += Input.GetAxis("Mouse X") * 300 * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            //transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
