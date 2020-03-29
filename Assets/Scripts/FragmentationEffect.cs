using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationEffect : MonoBehaviour
{
    public int numFragments;

    private Object fragPrefab;
    private GameObject frag;
    public void SpawnFragmentationEffect()
    {
        fragPrefab = Resources.Load("Effects/FragmentationEffect");
        frag = (GameObject)Instantiate(fragPrefab, transform.position, transform.rotation);
        frag.GetComponent<FragmentMaterializer>().EffectColorizer(transform.root.GetComponent<SpawnableGO>().kindOfSpawnable);

        ParticleSystem ps = frag.GetComponent<ParticleSystem>();

        var psMain = ps.main;
        psMain.maxParticles = numFragments;

        var psShape = ps.shape;
        psShape.enabled = true;
        psShape.shapeType = ParticleSystemShapeType.Mesh;
        psShape.mesh = GetComponent<MeshFilter>().mesh;


       
    }
}
