using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject shield;
    public GameObject lift;

    private Vector3 shieldShrunkScale;
    private Vector3 shieldOriginalScale;

    // Start is called before the first frame update
    void Start()
    {
        shieldOriginalScale = shield.transform.localScale;
        shieldShrunkScale = new Vector3(shieldOriginalScale.x, shieldOriginalScale.y / 4, shieldOriginalScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.GetComponent<EPlayerController>())
        {
            StopAllCoroutines();
            StartCoroutine(ShrinkShield());
        }
        else
        {
            Debug.Log("FOUND " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.GetComponent<EPlayerController>())
        {
            StopAllCoroutines();
            StartCoroutine(ExpandShield());
        }
    }

    IEnumerator ShrinkShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldShrunkScale) > 1)
        {
            Debug.Log("Shrink");
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldShrunkScale, 10 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ExpandShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldOriginalScale) > 1)
        {
            Debug.Log("ExpandShield");
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldOriginalScale, 10 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
        shield.transform.localScale = shieldOriginalScale;
    }
}
