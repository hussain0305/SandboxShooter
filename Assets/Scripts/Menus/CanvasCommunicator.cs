using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCommunicator : MonoBehaviour
{
    public GameObject constructionMenu;
    public Text alertMessage;
    public Text instructionMessage;
    public Slider energyBar;
    public GameObject disbalanceBar;
    public GameObject scoreboard;

    public GameObject energyBarMain;
    public GameObject energyDroppedMain;
    public RectTransform quickBuildMenuRect;

    public GameObject respawnScreen;
    public GameObject inGameScreen;
    public GameObject pauseMenuScreen;
    public Text respawningInSeconds;

    private float originalAnchoredY;

    private void Awake()
    {
        originalAnchoredY = quickBuildMenuRect.anchoredPosition.y;
    }
    public void ShowRespawnScreen(float screenDuration)
    {
        inGameScreen.SetActive(false);
        respawnScreen.SetActive(true);
        StartCoroutine(RespawnCountdown(screenDuration));
    }

    public void ShowInGameScreen()
    {
        respawnScreen.SetActive(false);
        inGameScreen.SetActive(true);
    }

    public void SetRespawningInSeconds(int sec)
    {
        respawningInSeconds.text = "" + sec;
    }

    IEnumerator RespawnCountdown(float countdownDuration)
    {
        float timeElapsed = 0;
        while (timeElapsed < countdownDuration)
        {
            timeElapsed += 1;
            SetRespawningInSeconds((int)countdownDuration - (int)timeElapsed);
            yield return new WaitForSeconds(1);
        }
        ShowInGameScreen();
    }

    public void SetEnergyDroppedMessage(bool val)
    {
        energyBarMain.SetActive(!val);
        energyDroppedMain.SetActive(val);

        if (val)
        {
            quickBuildMenuRect.anchoredPosition = new Vector2(quickBuildMenuRect.anchoredPosition.x, -5000);
        }

        else
        {
            quickBuildMenuRect.anchoredPosition = new Vector2(quickBuildMenuRect.anchoredPosition.x, originalAnchoredY);
        }
    }

}
