using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkMarker : MonoBehaviour
{
    public LineRenderer line;
    private RaycastHit hit;
    private Vector3 startOfLine;

    private void Update()
    {
        startOfLine = line.gameObject.transform.position;

        if (Physics.Raycast(startOfLine, -1 * transform.up, out hit, (1 << LayerMask.NameToLayer("ProximitySensor") | 1 << LayerMask.NameToLayer("BuildingDetector"))))
        {
            line.SetPosition(0, startOfLine);
            line.SetPosition(1, hit.point);         
        }
    }
}
