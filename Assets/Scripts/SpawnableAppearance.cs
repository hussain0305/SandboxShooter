using System.Collections;
using UnityEngine;

public class SpawnableAppearance : SpawnableComponentBase
{
    [Header("Appearance")]
    public Color damageColor;
    public Material formationMat;
    public Material finalMat;
    public Renderer[] allBodyRenderers;
    [HideInInspector]
    public Color[] rendererOriginalColors;

    const float FLICKER_SPEED = 10;
    private Color tColor;

    new void Start()
    {
        base.Start();
        StoreOriginalColors();
        StartDissolving();
    }

    void StoreOriginalColors()
    {
        rendererOriginalColors = new Color[allBodyRenderers.Length];
        int loop = 0;
        foreach (Renderer curr in allBodyRenderers)
        {
            rendererOriginalColors[loop] = allBodyRenderers[loop].material.color;
            loop++;
        }
    }
    public void StartDissolving()
    {
        int loop = 0;
        foreach(Renderer curr in allBodyRenderers)
        {
            curr.material = new Material(formationMat);
            curr.material.SetColor("_FillColor", rendererOriginalColors[loop]);
            loop++;
        }
        StartCoroutine(DissolveIn());
    }

    IEnumerator DissolveIn()
    {
        float percentage = 0;
        while (percentage <= 1)
        {
            percentage += 0.01f;
            foreach (Renderer curr in allBodyRenderers)
            {
                curr.material.SetFloat("_Percentage", percentage);
            }
            yield return new WaitForSeconds(parentSpawnable.buildTime / 100);
        }

        int loop = 0;
        foreach (Renderer curr in allBodyRenderers)
        {
            curr.material = new Material(finalMat);
            curr.material.SetColor("_Color", rendererOriginalColors[loop]);
            loop++;
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
        
        while (!CloseEnoughColors(allBodyRenderers[0].material.color, rendererOriginalColors[0]))
        {
            allBodyRenderers[0].material.color = Color.Lerp(allBodyRenderers[0].material.color, rendererOriginalColors[0], FLICKER_SPEED * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        allBodyRenderers[0].material.color = rendererOriginalColors[0];
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
