using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public Renderer rend;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        rend.material = new Material(mat);
        StartCoroutine(FormPlatformGrid());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FormPlatformGrid()
    {
        float formation = 0;
        while (formation <= 1)
        {
            rend.material.SetFloat("_FormLine", formation);
            formation += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
