using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragmenter : MonoBehaviour
{
    const float X_CORRECTION = 0.5f;
    const float Y_CORRECTION = 0.5f;
    const float Z_CORRECTION = 0.25f;

    private float yCenter;
    private float xSize, ySize, zSize;
    private float xCurr, yCurr, zCurr;

    private Vector3 currentRowCenter;
    private Vector3 verticalForceAddition;
    private GameObject spawnedFragment;
    private Material bodyMaterialToApply;
    private Material trailMaterialToApply;

    private void Start()
    {
        currentRowCenter = Vector3.zero;
        verticalForceAddition = new Vector3(0, 1000, 0);
    }
    public void FragmentThisSpawnable(KindOfSpawnable kind)
    {
        foreach(MeshRenderer currMesh in GetComponentsInChildren<MeshRenderer>())
        {
            if (!currMesh.GetComponent<IgnoreThisFor>() || (currMesh.GetComponent<IgnoreThisFor>() && !currMesh.GetComponent<IgnoreThisFor>().fragmentation))
            {
                FragmentThisMesh(currMesh, kind);
            }
        }
    }

    void FragmentThisMesh(MeshRenderer meshToFragment, KindOfSpawnable kind)
    {
        bodyMaterialToApply = meshToFragment.gameObject.transform.root.GetComponent<SpawnableAppearance>().finalMat;
        trailMaterialToApply = meshToFragment.gameObject.transform.root.GetComponent<SpawnableAppearance>().formationMat;

        xSize = meshToFragment.transform.root.localScale.x * meshToFragment.transform.localScale.x * 
            meshToFragment.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x;
        ySize = meshToFragment.transform.root.localScale.y * meshToFragment.transform.localScale.y *
            meshToFragment.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y;
        zSize = meshToFragment.transform.root.localScale.z * meshToFragment.transform.localScale.z *
            meshToFragment.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z;

        yCenter = meshToFragment.bounds.center.y;

        for (yCurr = yCenter - (ySize / 2); yCurr < yCenter + (ySize / 2); yCurr += 2)
        {
            for (zCurr = -zSize/2; zCurr < zSize/2; zCurr += 2)
            {
                currentRowCenter = meshToFragment.bounds.center;
                currentRowCenter.y = yCurr + Y_CORRECTION;
                currentRowCenter = (currentRowCenter + ((zCurr + Z_CORRECTION) * meshToFragment.gameObject.transform.forward));
                for (xCurr = -xSize/2; xCurr < xSize/2; xCurr += 2)
                {
                    if (Random.Range(1, 10) < 6)
                    {
                        spawnedFragment = ObjectPooler.CentralObjectPool.SpawnFromPool("Fragment",
                            currentRowCenter + ((xCurr + X_CORRECTION) * meshToFragment.gameObject.transform.right),
                            meshToFragment.transform.root.rotation);
                        spawnedFragment.GetComponent<FragmentMaterializer>().ApplyMaterial(kind);
                        spawnedFragment.GetComponent<Rigidbody>().AddForce(Random.Range(1, 10) * 200 * ((spawnedFragment.transform.position - meshToFragment.bounds.center) + verticalForceAddition));
                    
                    }
                }
            }
        }
    }
}
