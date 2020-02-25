using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    const float ALERT_MSG_DURATION = 2;

    private Text alertMessage;
    private Text instructionMessage;
    private GameObject constructionMenu;
    private GameObject scoreboard;

    private Slider energyBar;
    private GameObject disbalanceBar;

    private CanvasCommunicator canvasCom;
    private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        FetchUIComponents();
        Cursor.visible = false;
    }

    void FetchUIComponents()
    {
        canvasCom = GameObject.FindObjectOfType<CanvasCommunicator>();
        if (!canvasCom)
        {
            return;
        }

        energyBar = canvasCom.energyBar;
        alertMessage = canvasCom.alertMessage;
        instructionMessage = canvasCom.instructionMessage;
        constructionMenu = canvasCom.constructionMenu;
        disbalanceBar = canvasCom.disbalanceBar;
        scoreboard = canvasCom.scoreboard;
        pauseMenu = canvasCom.pauseMenuScreen;

        alertMessage.gameObject.SetActive(false);
        instructionMessage.gameObject.SetActive(false);
        constructionMenu.SetActive(false);
        disbalanceBar.SetActive(false);
        pauseMenu.SetActive(false);

        constructionMenu.GetComponent<ConstructionMenu>().ForceStart();
    }

    public void ToggleConstructionMenu()
    {
        constructionMenu.SetActive(!constructionMenu.activeSelf);
        constructionMenu.GetComponent<ConstructionMenu>().quickMenu.gameObject.SetActive(!constructionMenu.activeSelf);
        Cursor.visible = constructionMenu.activeSelf;
    }

    public void QuickMenuScroll(float val)
    {
        if (val < 0)
        {
            constructionMenu.GetComponent<ConstructionMenu>().ScrollDownQuickList();
        }
        if (val > 0)
        {
            constructionMenu.GetComponent<ConstructionMenu>().ScrollUpQuickList();
        }
    }

    public void QuickConstruct()
    {
        constructionMenu.GetComponent<ConstructionMenu>().QuickConstruct();
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
        energyBar.value = (float)value / 1000;
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

    public void ShowRespawnScreen(float screenDuration)
    {
        canvasCom.ShowRespawnScreen(screenDuration);
    }

    public void ToggleScoreboard()
    {
        scoreboard.SetActive(!scoreboard.activeSelf);
    }

    public void SetScoreboardActive(bool val)
    {
        scoreboard.SetActive(val);
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

}
