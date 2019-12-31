using Photon.Pun;
using System.IO;
using UnityEngine;
public class EPlayerNetworkPresence : MonoBehaviour
{
    private PhotonView pView;

    private EPlayerController playerController;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        if (pView.IsMine)
        {
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerCharacter"), 
                GameObject.FindObjectOfType<GameManager>().GetSpawnLocation(), Quaternion.identity, 0);
            playerController = spawnedPlayer.GetComponent<EPlayerController>();
        }
    }
}