using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCommunicator : MonoBehaviour
{
    public GameObject constructionMenu;
    public Text alertMessage;
    public Text instructionMessage;
    public Text energyValue;


    public GameObject GetConstructionMenuComponent()
    {
        return constructionMenu;
    }

    public Text GetAlertMessageComponent()
    {
        return alertMessage;
    }

    public Text GetInstructionMessageComponent()
    {
        return instructionMessage;
    }

    public Text GetEnergyValueComponent()
    {
        return energyValue;
    }

}
