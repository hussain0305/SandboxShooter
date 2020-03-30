using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationEffect : MonoBehaviour
{
    public int numFragments;
    public KindOfSpawnable kind;
    private Object fragPrefab;
    private GameObject frag;
    public void SpawnFragmentationEffect()
    {
        fragPrefab = Resources.Load("Effects/FragmentationEffect");
        frag = (GameObject)Instantiate(fragPrefab, transform.position, transform.rotation);

        frag.transform.localScale = transform.localScale;
        frag.GetComponent<FragmentMaterializer>().EffectColorizer(kind);

        ParticleSystem ps = frag.GetComponent<ParticleSystem>();

        var psMain = ps.main;
        psMain.maxParticles = 3 * numFragments;

        var psShape = ps.shape;
        psShape.enabled = true;
        psShape.shapeType = ParticleSystemShapeType.Mesh;
        psShape.mesh = GetComponent<MeshFilter>().mesh;
    }
}
