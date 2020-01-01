using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject offence;
    public GameObject defence;
    public GameObject decorations;

    [Header("Buttons")]
    public Button offenceButton;
    public Button defenceButton;
    public Button decorationsButton;

    [Header("Misc")]
    public UIInfoPopup popup;

    public Material highlightedCategoryMaterial;
    public Material regularCategoryMaterial;

    private GameManager gameManager;
    private EPlayerController player;

    public void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        player = gameManager.FetchLocalPlayer();

        highlightedCategoryMaterial = new Material(highlightedCategoryMaterial);
        regularCategoryMaterial = new Material(regularCategoryMaterial);
    }
    void OnEnable()
    {
        popup.gameObject.SetActive(false);
        offence.SetActive(false);
        defence.SetActive(false);
        decorations.SetActive(false);
        
        defenceButton.GetComponent<Image>().material = regularCategoryMaterial;
        decorationsButton.GetComponent<Image>().material = regularCategoryMaterial;
        offenceButton.GetComponent<Image>().material = regularCategoryMaterial;
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

        defenceButton.GetComponent<Image>().material = regularCategoryMaterial;
        decorationsButton.GetComponent<Image>().material = regularCategoryMaterial;
        offenceButton.GetComponent<Image>().material = highlightedCategoryMaterial;
    }

    public void DefenceSelected()
    {
        offence.SetActive(false);
        decorations.SetActive(false);

        defence.SetActive(true);

        offenceButton.GetComponent<Image>().material = regularCategoryMaterial;
        decorationsButton.GetComponent<Image>().material = regularCategoryMaterial;
        defenceButton.GetComponent<Image>().material = highlightedCategoryMaterial;
    }

    public void DecorationSelected()
    {
        offence.SetActive(false);
        defence.SetActive(false);

        decorations.SetActive(true);

        offenceButton.GetComponent<Image>().material = regularCategoryMaterial;
        defenceButton.GetComponent<Image>().material = regularCategoryMaterial;
        decorationsButton.GetComponent<Image>().material = highlightedCategoryMaterial;
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
                gameManager.SpawnPlatform(tSpawn.pathStrings, loc);
                //GameObject spawnedSpawnable = Instantiate(tSpawn.prefab, loc, Quaternion.identity);
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
