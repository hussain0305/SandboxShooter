﻿using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameTip
{
    public Text buttonName;
    public UIPanelSlider correspondingSlider;
    public bool isOut;
}
public class HowToPlay : MonoBehaviour
{
    public Transform allTipsCluster;

    private GameTip[] allTips;

    private void Awake()
    {
        PrepareList();
    }

    public void PrepareList()
    {
        allTips = new GameTip[allTipsCluster.childCount];
        foreach (Transform currTip in allTipsCluster)
        {
            allTips[currTip.GetSiblingIndex()].buttonName = currTip.GetChild(0).GetChild(0).GetComponent<Text>();
            allTips[currTip.GetSiblingIndex()].correspondingSlider = currTip.GetComponentInChildren<UIPanelSlider>();
            allTips[currTip.GetSiblingIndex()].isOut = true;
        }
    }
    public void OnEnable()
    {
        ResetEverything();
    }

    void ResetEverything()
    {
        for (int loop = 0; loop < allTips.Length; loop++)
        {
            if (!allTips[loop].isOut)
            {
                allTips[loop].correspondingSlider.ResetPosition();
                allTips[loop].buttonName.color = Color.white;
                allTips[loop].isOut = true;
            }
        }
    }

    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


    public void TogglePanel(int panelNo)
    {
        if (allTips[panelNo].isOut)
        {
            allTips[panelNo].correspondingSlider.MoveIn();
            allTips[panelNo].buttonName.color = Color.green;
        }
        else
        {
            allTips[panelNo].correspondingSlider.MoveOut();
            allTips[panelNo].buttonName.color = Color.white;
        }
        allTips[panelNo].isOut = !allTips[panelNo].isOut;
    }

    public void TipSelected(int tipNo)
    {
        for(int loop = 0; loop < allTips.Length; loop++)
        {
            if (!allTips[loop].isOut)
            {
                allTips[loop].correspondingSlider.MoveOut();
                allTips[loop].buttonName.color = Color.white;
                allTips[loop].isOut = true;
            }
        }
        
        allTips[tipNo].correspondingSlider.MoveIn();
        allTips[tipNo].buttonName.color = Color.green;
        allTips[tipNo].isOut = false;

    }
}
