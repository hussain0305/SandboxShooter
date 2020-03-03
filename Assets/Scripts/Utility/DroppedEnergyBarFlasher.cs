using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedEnergyBarFlasher : MonoBehaviour
{
    private Coroutine flashRoutine;

    public void OnEnable()
    {
        flashRoutine = StartCoroutine(FlashMessage());
    }

    public void OnDisable()
    {
        StopCoroutine(flashRoutine);
    }
    IEnumerator FlashMessage()
    {
        while (true)
        {
            foreach(Transform currChild in transform)
            {
                currChild.gameObject.SetActive(!currChild.gameObject.activeSelf);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
