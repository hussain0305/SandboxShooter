using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;
public class EPlayerNetworkPresence : MonoBehaviour
{
    [HideInInspector]
    public PlayerRecord gameRecord;

    private PhotonView pView;

    private EPlayerController playerController;

    private GameObject spawnedPlayer;

    private void Start()
    {
        //you can add delays here
        pView = GetComponent<PhotonView>();
        if (pView.IsMine)
        {
            SpawnPlayer();
        }

        gameRecord = new PlayerRecord(this);
    }

    void SpawnPlayer()
    {
        spawnedPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerCharacter"),
                GameObject.FindObjectOfType<GameManager>().GetSpawnLocation(), Quaternion.identity, 0);

        playerController = spawnedPlayer.GetComponent<EPlayerController>();
        playerController.SetNetworkPresence(this, pView.ViewID);
    }

    public void StartRespawnCooldown(float duration)
    {
        if (pView.IsMine)
        {
            StartCoroutine(Countdown(duration));
        }
    }

    public EPlayerController GetPlayerController()
    {
        return playerController;
    }

    IEnumerator Countdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        SpawnPlayer();
    }

    public int GetID()
    {
        return pView.ViewID;
    }

    public void KilledPlayer()
    {
        gameRecord.AddKill();
        //if (pView.IsMine)
        //{
        //    pView.RPC("RPC_KilledPlayer", RpcTarget.All);
        //}
    }

    //[PunRPC]
    //void RPC_KilledPlayer()
    //{
    //    gameRecord.AddKill();
    //}

    public void WasKilled()
    {
        gameRecord.AddDeath();
        //if (pView.IsMine)
        //{
        //    pView.RPC("RPC_WasKilled", RpcTarget.All);
        //}
    }

    //[PunRPC]
    //void RPC_WasKilled()
    //{
    //    gameRecord.AddDeath();
    //}

    public void BrokeSpawnable()
    {
        gameRecord.AddSpawnableBroken();
        //if (pView.IsMine)
        //{
        //    pView.RPC("RPC_BrokeSpawnable", RpcTarget.All);
        //}
    }

    //[PunRPC]
    //void RPC_BrokeSpawnable()
    //{
    //    gameRecord.AddSpawnableBroken();
    //}

    public bool IsLocal()
    {
        return pView.IsMine;
    }
}