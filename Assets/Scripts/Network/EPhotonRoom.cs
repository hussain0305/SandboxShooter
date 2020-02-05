using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EPhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public int[] gameScenes;
    [HideInInspector]
    public int currentScene;

    public static EPhotonRoom thisRoom;
    private PhotonView pView;

    private int selectedScene;
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
        if (IsAGameScene(scene.buildIndex))
        {
            CreatePlayer();
        }
    }

    bool IsAGameScene(int currentScene)
    {
        foreach(int currSceneNumber in gameScenes)
        {
            if (currentScene == currSceneNumber)
            {
                return true;
            }
        }
        return false;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(gameScenes[selectedScene]);
    }

    public void MapSelected(int index)
    {
        selectedScene = index;
    }

    public void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerPresence"), transform.position, Quaternion.identity, 0);
    }
}
