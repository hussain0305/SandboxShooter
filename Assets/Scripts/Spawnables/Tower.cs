using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject shield;

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
            if (shield)
            {
                StartCoroutine(ShrinkShield());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.GetComponent<EPlayerController>())
        {
            StopAllCoroutines();
            if (shield)
            {
                StartCoroutine(ExpandShield());
            }
        }
    }

    IEnumerator ShrinkShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldShrunkScale) > 0.1f)
        {
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldShrunkScale, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ExpandShield()
    {
        while (Vector3.Distance(shield.transform.localScale, shieldOriginalScale) > 0.1f)
        {
            shield.transform.localScale = Vector3.Lerp(shield.transform.localScale, shieldOriginalScale, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        shield.transform.localScale = shieldOriginalScale;
    }
}
