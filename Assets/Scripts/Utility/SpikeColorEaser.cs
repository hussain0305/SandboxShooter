using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeColorEaser : MonoBehaviour
{
    private Vector3 finalScale;
    void Start()
    {
        finalScale = new Vector3(1, 1, 1);
        StartCoroutine(EaseIn());
    }
    IEnumerator EaseIn()
    {
        transform.localScale = Vector3.zero;
        while(Vector3.Distance(transform.localScale, finalScale) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);

        GetComponent<Rigidbody>().velocity = 12 * GetComponent<Rigidbody>().velocity;

        finalScale = Vector3.zero;

        yield return new WaitForSeconds(1.75f);

        while (Vector3.Distance(transform.localScale, finalScale) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = finalScale;
    }

}
