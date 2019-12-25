using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstructionMenu : MonoBehaviour
{
    public GameObject offence;
    public GameObject defence;
    public GameObject decorations;
    public UIInfoPopup popup;

    private GameManager gameManager;
    private EPlayerController player;

    public void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindObjectOfType<EPlayerController>();
    }
    void OnEnable()
    {
        popup.gameObject.SetActive(false);
        offence.SetActive(false);
        defence.SetActive(false);
        decorations.SetActive(false);
    }

    private void OnDisable()
    {
        popup.gameObject.SetActive(false);
    }

    public void OffenceSelected()
    {
        defence.SetActive(false);
        decorations.SetActive(false);

        offence.SetActive(true);
    }

    public void DefenceSelected()
    {
        offence.SetActive(false);
        decorations.SetActive(false);

        defence.SetActive(true);
    }

    public void DecorationSelected()
    {
        offence.SetActive(false);
        defence.SetActive(false);

        decorations.SetActive(true);
    }

    public void ConstructionSelected(SpawnableType type, string id)
    {
        Vector3 loc = new Vector3(0, 0, 0);
        Quaternion rot = Quaternion.identity;
        if(player.GetSpawnLocationAndRotation(out loc, out rot))
        {
            player.ToggleConstructionMenu();
            Spawnable tSpawn = gameManager.GetBlueprint(type, id);
            if (tSpawn.constructionEnergyRequired < player.playerEnergy.GetEnergy())
            {
                player.playerEnergy.SpendEnergy(tSpawn.constructionEnergyRequired);
                gameManager.SpawnSpawnable(type, id, loc, rot, player);
            }
            else
            {
                player.playerUI.DisplayAlertMessage("You don't have enough energy");
            }
        }
    }
    public void PlatformSelected()
    {
        Vector3 loc = new Vector3(0, 0, 0);
        Quaternion rot = Quaternion.identity;
        if (player.GetSpawnLocationAndRotation(out loc, out rot))
        {
            player.ToggleConstructionMenu();
            Spawnable tSpawn = gameManager.GetBlueprint(SpawnableType.Decoration, "FloatingPlatform");
            if (tSpawn.constructionEnergyRequired < player.playerEnergy.GetEnergy())
            {
                player.playerEnergy.SpendEnergy(tSpawn.constructionEnergyRequired);

                GameObject spawnedSpawnable = Instantiate(tSpawn.prefab, loc, Quaternion.identity);
            }
            else
            {
                player.playerUI.DisplayAlertMessage("You don't have enough energy");
            }
        }
    }
    

    public UIInfoPopup GetPopup()
    {
        return popup;
    }
}
