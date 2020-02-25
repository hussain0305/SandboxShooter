using UnityEngine;
using UnityEngine.UI;

public class Scene_MainMenu : MonoBehaviour
{
    public GameObject createPanel;
    public InputField serverName;
    public Slider numPlayers;
    public Text numPlayersValue;

    public GameObject networkMain;
    private void Start()
    {
        createPanel.SetActive(false);
    }
    public void ToggleCreateServerPanel()
    {
        createPanel.SetActive(!createPanel.activeSelf);
    }

    public void CreateRoom()
    {
        string servName = (serverName.text == "") ? ("SS Server " + Random.Range(1, 5)) : serverName.text;

        int numPlayer = (int)numPlayers.value;

        networkMain.GetComponentInChildren<EPhotonLobby>().CreateRoom(servName, numPlayer);
    }

    public void JoinRoom()
    {
        networkMain.GetComponentInChildren<EPhotonLobby>().AttemptJoinRoom();
    }

    public void OnNumPlayersChanged(float val)
    {
        numPlayersValue.text = "" + val;
    }
}
