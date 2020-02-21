using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class EPhotonLobby : MonoBehaviourPunCallbacks
{
    public static EPhotonLobby lobby;

    public Text regionText;
    public GameObject splashScreen;


    private RoomInfo[] rooms;

    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        WasConnected();
    }

    public void AttemptJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        int randomRoom = Random.Range(0, 1000);
        RoomOptions roomSettings = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Sandbox FPS Server " + randomRoom, roomSettings);
    }

    public void CreateRoom(string roomName, int numPlayers)
    {
        RoomOptions roomSettings = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)numPlayers };
        PhotonNetwork.CreateRoom(roomName, roomSettings);
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
        //base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
        //base.OnCreateRoomFailed(returnCode, message);
    }


    void WasConnected()
    {
        regionText.text = "Region : " + PhotonNetwork.CloudRegion;
        regionText.color = Color.green;

        splashScreen.SetActive(false);
    }
}
