using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGraph : MonoBehaviour
{
    public GameObject visualizationObject;

    //Instantiate One object per X all along the line.
    //Have a "motion" factor
    //y will be calculate based on position and motion factor;

    public int numPoints;
    public int numPatterns;

    private void Start()
    {
        SpawnGraphPoints();

        PlotEquation();

        StartCoroutine(MoveGraph());

        GetComponent<Rigidbody>().velocity = transform.forward * 5;
    }

    void SpawnGraphPoints()
    {
        GameObject currPoint;
        for(int loop = 0; loop < numPoints; loop++)
        {
            currPoint = Instantiate(visualizationObject);
            currPoint.transform.SetParent(this.transform);
            currPoint.transform.localRotation = Quaternion.identity;
            currPoint.transform.localPosition = new Vector3(0, 0, loop);
        }
    }

    void PlotEquation()
    {
        float angle = 0;
        foreach(Transform currPoint in transform)
        {
            currPoint.transform.localPosition = new Vector3(currPoint.transform.localPosition.x, 10 * Mathf.Sin(angle), currPoint.transform.localPosition.z);
            //currPoint.GetComponent<LerpedMovement>().GoToPosition(new Vector3(currPoint.transform.localPosition.x, 10 * Mathf.Sin(angle), currPoint.transform.localPosition.z));
            angle += ((float)numPoints / numPatterns) / 360;
        }
    }

    IEnumerator MoveGraph()
    {
        int nextIndex = 0;
        while (true)
        {
            foreach(Transform currPoint in transform)
            {
                nextIndex = currPoint.transform.GetSiblingIndex() + 1;
                if (nextIndex == transform.childCount)
                {
                    nextIndex = 0;
                }
                //currPoint.transform.localPosition = new Vector3(currPoint.transform.localPosition.x, transform.GetChild(nextIndex).transform.localPosition.y, currPoint.transform.localPosition.z);
                currPoint.GetComponent<LerpedMovement>().GoToPosition(new Vector3(currPoint.transform.localPosition.x, transform.GetChild(nextIndex).transform.localPosition.y, currPoint.transform.localPosition.z));
            }

            yield return new WaitForSeconds(1);
        }
    }
}
