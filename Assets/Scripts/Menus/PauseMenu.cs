using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks, IConnectionCallbacks
{

    public void ExitToMainMenu()
    {
        StartCoroutine(StartDisconnecting());
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
}
