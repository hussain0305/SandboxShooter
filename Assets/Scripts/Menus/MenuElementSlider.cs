using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MenuSlidingElement
{
    public Text buttonName;
    public UIPanelSlider correspondingSlider;
    public bool isOut;
}
public class MenuElementSlider : MonoBehaviour
{
    public Transform allElementsCluster;

    private MenuSlidingElement[] allElements;

    private void Awake()
    {
        PrepareList();
    }

    public void PrepareList()
    {
        allElements = new MenuSlidingElement[allElementsCluster.childCount];
        foreach (Transform currTip in allElementsCluster)
        {
            allElements[currTip.GetSiblingIndex()].buttonName = currTip.GetChild(0).GetChild(0).GetComponent<Text>();
            allElements[currTip.GetSiblingIndex()].correspondingSlider = currTip.GetComponentInChildren<UIPanelSlider>();
            allElements[currTip.GetSiblingIndex()].isOut = true;
        }
    }
    public void OnEnable()
    {
        ResetEverything();
    }

    void ResetEverything()
    {
        for (int loop = 0; loop < allElements.Length; loop++)
        {
            if (!allElements[loop].isOut)
            {
                allElements[loop].correspondingSlider.ResetPosition();
                allElements[loop].buttonName.color = Color.white;
                allElements[loop].isOut = true;
            }
        }
    }

    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


    public void TogglePanel(int panelNo)
    {
        if (allElements[panelNo].isOut)
        {
            allElements[panelNo].correspondingSlider.MoveIn();
            allElements[panelNo].buttonName.color = Color.green;
        }
        else
        {
            allElements[panelNo].correspondingSlider.MoveOut();
            allElements[panelNo].buttonName.color = Color.white;
        }
        allElements[panelNo].isOut = !allElements[panelNo].isOut;
    }

    public void ElementSelected(int tipNo)
    {
        for (int loop = 0; loop < allElements.Length; loop++)
        {
            if (!allElements[loop].isOut && loop != tipNo)
            {
                allElements[loop].correspondingSlider.MoveOut();
                allElements[loop].buttonName.color = Color.white;
                allElements[loop].isOut = true;
            }
        }

        TogglePanel(tipNo);
        //allElements[tipNo].correspondingSlider.MoveIn();
        //allElements[tipNo].buttonName.color = Color.green;
        //allElements[tipNo].isOut = false;

    }
}
