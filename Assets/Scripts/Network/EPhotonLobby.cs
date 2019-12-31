using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EPhotonLobby : MonoBehaviourPunCallbacks
{
    public static EPhotonLobby lobby;

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
    }

    public void AttemptJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        int randomRoom = Random.Range(0, 1000);
        RoomOptions roomSettings = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Sandbox Shooter Server " + randomRoom, roomSettings);
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
}
