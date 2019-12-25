using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(50 * new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)));
        StartCoroutine(DiePlease());
    }

    IEnumerator DiePlease()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
