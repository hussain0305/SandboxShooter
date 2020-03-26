using System.Collections;
using UnityEngine;

public class Expiry : MonoBehaviour
{
    public float lifetime;

    private void Start()
    {
        StartCoroutine(SuicidePlease());
    }

    IEnumerator SuicidePlease()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
