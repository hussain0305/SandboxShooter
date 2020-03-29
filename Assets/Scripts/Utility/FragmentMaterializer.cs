using System.Collections;
using UnityEngine;

public class FragmentMaterializer : MonoBehaviour
{
    public FragmentColorInfo[] fragmentColorInformation;

    public void EffectColorizer(KindOfSpawnable kind)
    {
        foreach (FragmentColorInfo currColorInfo in fragmentColorInformation)
        {
            if (currColorInfo.kindOfSpawnable == kind)
            {
                GetComponent<ParticleSystemRenderer>().material = new Material(currColorInfo.bodyMaterial);
                GetComponent<ParticleSystemRenderer>().trailMaterial = new Material(currColorInfo.trailMaterial);
                break;
            }
        }
    }

}


//public void InitializeFragment(KindOfSpawnable kind, Vector3 force)
//{
//    ApplyMaterial(kind);
//    StartCoroutine(DelayedForce(force));
//}

//IEnumerator DelayedForce(Vector3 force)
//{
//    yield return new WaitForSeconds(0.1f);
//    ApplyForce(force);
//}
//public void ApplyMaterial(KindOfSpawnable kind)
//{
//    foreach (FragmentColorInfo currColorInfo in fragmentColorInformation)
//    {
//        if (currColorInfo.kindOfSpawnable == kind)
//        {
//            Material bMat = new Material(currColorInfo.bodyMaterial);
//            Material tMat = new Material(currColorInfo.trailMaterial);
//            break;
//        }
//    }
//}

//public void ApplyForce(Vector3 force)
//{
//    GetComponent<Rigidbody>().useGravity = true;

//    GetComponent<Rigidbody>().AddForce(force);
//}