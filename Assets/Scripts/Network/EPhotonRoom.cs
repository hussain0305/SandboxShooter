using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EPhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public int gameScene;
    [HideInInspector]
    public int currentScene;

    public static EPhotonRoom thisRoom;
    private PhotonView pView;
    void Awake()
    {
        if (thisRoom != null && thisRoom != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            EPhotonRoom.thisRoom = this;
        }

        pView = GetComponent<PhotonView>();

    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        StartGame();
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == gameScene)
        {
            CreatePlayer();
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(gameScene);
    }

    public void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerPresence"), transform.position, Quaternion.identity, 0);
    }
}
