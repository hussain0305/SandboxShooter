using System.Collections;
using UnityEngine;

public class ExpiryPoolObject : MonoBehaviour
{
    public float lifetime;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(SuicidePlease());
    }

    IEnumerator SuicidePlease()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
