using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class SplashScreen : MonoBehaviourPunCallbacks
{
    public Text regionText;
    public GameObject splashScreen;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        WasConnected();
    }
    void WasConnected()
    {
        regionText.text = "Region : " + PhotonNetwork.CloudRegion;
        regionText.color = Color.green;

        splashScreen.SetActive(false);
    }

}
