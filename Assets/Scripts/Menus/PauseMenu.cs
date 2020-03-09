using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public GameObject settingsScreen;
    public override void OnEnable()
    {
        base.OnEnable();
        Cursor.visible = true;
        SetSettingsScreen(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Cursor.visible = false;
    }
    public void ExitToMainMenu()
    {
        StartCoroutine(StartDisconnecting());
    }

    public void ToggleSettingsScreen()
    {
        settingsScreen.SetActive(!settingsScreen.activeSelf);
    }

    IEnumerator StartDisconnecting()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }


    public void SetSettingsScreen(bool val)
    {
        settingsScreen.SetActive(val);
    }
}
