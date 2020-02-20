using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SplashScreen : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public override void OnJoinedLobby()
    {
        Debug.Log("adsad");
        Destroy(this.gameObject);
    }
}
