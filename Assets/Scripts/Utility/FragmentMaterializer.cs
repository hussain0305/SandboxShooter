using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentMaterializer : MonoBehaviour
{
    public MeshRenderer body;
    public TrailRenderer trail;
    public FragmentColorInfo[] fragmentColorInformation;

    public void ApplyMaterial(KindOfSpawnable kind)
    {
        foreach(FragmentColorInfo currColorInfo in fragmentColorInformation)
        {
            if(currColorInfo.kindOfSpawnable == kind)
            {
                body.material = currColorInfo.bodyMaterial;
                trail.material = currColorInfo.trailMaterial;
            }

            break;
        }
    }

}
