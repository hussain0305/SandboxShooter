using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    const float ALERT_MSG_DURATION = 2;

    public Text alertMessage;
    public Text instructionMessage;
    public GameObject constructionMenu;
    
    public Text energyValue;

    // Start is called before the first frame update
    void Start()
    {
        alertMessage.gameObject.SetActive(false);
        instructionMessage.gameObject.SetActive(false);
        constructionMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void ToggleConstructionMenu()
    {
        constructionMenu.SetActive(!constructionMenu.activeSelf);
        Cursor.visible = constructionMenu.activeSelf;
    }

    #region Energy Bar

    public void UpdateEnergyBar(int value)
    {
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
