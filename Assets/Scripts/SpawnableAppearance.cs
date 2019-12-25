using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableAppearance : SpawnableComponentBase
{
    [Header("Appearance")]
    public Color objectColor;
    public Color damageColor;
    public Color burnColor;
    public Material dissolveMat;
    public Material finalMat;
    public Renderer[] allBodyRenderers;

    const float FLICKER_SPEED = 10;
    private Color tColor;

    new void Start()
    {
        base.Start();
        StartDissolving();
    }

    public void StartDissolving()
    {
        foreach(Renderer curr in allBodyRenderers)
        {
            curr.material = new Material(dissolveMat);
            curr.material.SetColor("_Color", objectColor);
            curr.material.SetColor("_BurnColor", burnColor);
        }
        StartCoroutine(DissolveIn());
    }

    IEnumerator DissolveIn()
    {
        float burnAmount = 1;
        while (burnAmount > 0.5f)
        {
            burnAmount -= 0.01f;
            foreach (Renderer curr in allBodyRenderers)
            {
                curr.material.SetFloat("_SliceAmount", burnAmount);
            }

            yield return new WaitForSeconds(parentSpawnable.buildTime / 100);
        }
        foreach (Renderer curr in allBodyRenderers)
        {
            curr.material = new Material(finalMat);
            curr.material.SetColor("_Color", objectColor);
        }
        parentSpawnable.SpawnableIsReadyForBusiness();
    }

    public void WasHit()
    {
        StopAllCoroutines();
        StartCoroutine(DamageFlicker());
    }

    IEnumerator DamageFlicker()
    {
        while(!CloseEnoughColors(allBodyRenderers[0].material.color, damageColor))
        {
            allBodyRenderers[0].material.color = Color.Lerp(allBodyRenderers[0].material.color, damageColor, FLICKER_SPEED * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        allBodyRenderers[0].material.color = damageColor;
        
        yield return new WaitForSeconds(0.1f);
        
        while (!CloseEnoughColors(allBodyRenderers[0].material.color, objectColor))
        {
            allBodyRenderers[0].material.color = Color.Lerp(allBodyRenderers[0].material.color, objectColor, FLICKER_SPEED * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        allBodyRenderers[0].material.color = objectColor;
    }

    public bool CloseEnoughColors(Color c1, Color c2)
    {
        if((c1.r - c2.r < 10)&& (c1.g - c2.g < 10) && (c1.b - c2.b < 10) && (c1.a - c2.a < 10))
        {
            return true;
        }
        return false;
    }
}
