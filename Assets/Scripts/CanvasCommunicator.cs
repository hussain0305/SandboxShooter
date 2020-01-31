using UnityEngine;
using UnityEngine.UI;

public class CanvasCommunicator : MonoBehaviour
{
    public GameObject constructionMenu;
    public Text alertMessage;
    public Text instructionMessage;
    public Slider energyBar;
    public GameObject disbalanceBar;


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

    public Slider GetEnergyBarComponent()
    {
        return energyBar;
    }

    public GameObject GetDisbalanceBar()
    {
        return disbalanceBar;
    }


}
