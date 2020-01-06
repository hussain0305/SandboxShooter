﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    const float ALERT_MSG_DURATION = 2;

    private Text alertMessage;
    private Text instructionMessage;
    private GameObject constructionMenu;
    
    private Text energyValue;
    private GameObject disbalanceBar;

    // Start is called before the first frame update
    void Start()
    {
        FetchUIComponents();
        Cursor.visible = false;
    }

    void FetchUIComponents()
    {
        CanvasCommunicator canvasCom = GameObject.FindObjectOfType<CanvasCommunicator>();
        if (!canvasCom)
        {
            return;
        }

        energyValue = canvasCom.GetEnergyValueComponent();
        alertMessage = canvasCom.GetAlertMessageComponent();
        instructionMessage = canvasCom.GetInstructionMessageComponent();
        constructionMenu = canvasCom.GetConstructionMenuComponent();
        disbalanceBar = canvasCom.GetDisbalanceBar();

        alertMessage.gameObject.SetActive(false);
        instructionMessage.gameObject.SetActive(false);
        constructionMenu.SetActive(false);
        disbalanceBar.SetActive(false);

    }

    public void ToggleConstructionMenu()
    {
        constructionMenu.SetActive(!constructionMenu.activeSelf);
        Cursor.visible = constructionMenu.activeSelf;
    }

    #region Disbalance Bar

    public void NewDisbalanceValueReceived(float disbalancePercentage)
    {
        if (!disbalanceBar.activeSelf)
        {
            disbalanceBar.SetActive(true);
        }
        
        disbalanceBar.GetComponentInChildren<Image>().material.SetFloat("_Percentage", disbalancePercentage);
        
        if(disbalancePercentage < 0.01f || disbalancePercentage > 1.00f)
        {
            disbalanceBar.SetActive(false);
        }
    }

    #endregion

    #region Energy Bar

    public void UpdateEnergyBar(int value)
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        energyValue.text = "" +value;
    }

    #endregion

    #region Alert Messages

    public void DisplayAlertMessage(string message)
    {
        alertMessage.gameObject.SetActive(true);
        alertMessage.text = message;
        StartCoroutine(FadeOutAlertMessage());
    }

    public void DisplayInstructionMessage(string message)
    {
        instructionMessage.gameObject.SetActive(true);
        instructionMessage.text = message;
    }

    public void RemoveInstructionMessage()
    {
        instructionMessage.gameObject.SetActive(false);
    }


    IEnumerator FadeOutAlertMessage()
    {
        //add fade out logic
        //capability to scroll through multiple messages
        yield return new WaitForSeconds(ALERT_MSG_DURATION);
        alertMessage.gameObject.SetActive(false);
    }

    #endregion
}
