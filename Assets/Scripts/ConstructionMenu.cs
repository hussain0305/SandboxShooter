using System.Collections;
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

    [Header("Quick Build Menu")]
    public Color highlightedColor;
    public QuickBuildMenu quickMenu;
    public Transform qSpawnables;

    private GameManager gameManager;
    private EPlayerController player;
    
    private int qCurrentCategory;
    private int qCurrentSpawnable;


    public void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        player = gameManager.FetchLocalPlayer();

        highlightedCategoryMaterial = new Material(highlightedCategoryMaterial);
        regularCategoryMaterial = new Material(regularCategoryMaterial);
    }

    public void ForceStart()
    {
        gameObject.SetActive(true);
        qCurrentCategory = 0;
        qCurrentSpawnable = 0;
        player = gameManager.FetchLocalPlayer();
        HighlightQuickSpawnable();
        quickMenu.ShowSelected();
        gameObject.SetActive(false);
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
        quickMenu.ShowSelected();
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
            if (gameObject.activeSelf)
            {
                player.ToggleConstructionMenu();
            }
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
            if (gameObject.activeSelf)
            {
                player.ToggleConstructionMenu();
            }
            Spawnable tSpawn = gameManager.GetBlueprint(SpawnableType.Decoration, "FloatingPlatform");
            if (tSpawn.constructionEnergyRequired < player.playerEnergy.GetEnergy())
            {
                player.playerEnergy.SpendEnergy(tSpawn.constructionEnergyRequired);
                loc.x = Mathf.Clamp(loc.x, -112.5f, 112.5f);
                loc.z = Mathf.Clamp(loc.z, -112.5f, 112.5f);
                gameManager.SpawnPlatform(tSpawn.pathStrings, loc);

                //Now Spawn in other quadrants as well.
                //Assuming loc to be ++, other position would be

                Vector3 tLoc = new Vector3(-1 * loc.x, loc.y, loc.z);
                gameManager.SpawnPlatform(tSpawn.pathStrings, tLoc);
                tLoc = new Vector3(loc.x, loc.y, -1 * loc.z);
                gameManager.SpawnPlatform(tSpawn.pathStrings, tLoc);
                tLoc = new Vector3(-1 * loc.x, loc.y, -1 * loc.z);
                gameManager.SpawnPlatform(tSpawn.pathStrings, tLoc);
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

    //=======QUICK CONSTRUCTION MENU STUFF BELOW===========

    public void ScrollDownQuickList()
    {
        if (!quickMenu.selectionBlock.activeSelf)
        {
            quickMenu.ShowSelections();
        }
        qCurrentSpawnable++;
        if(qCurrentSpawnable > qSpawnables.GetChild(qCurrentCategory).childCount - 1)
        {
            qCurrentSpawnable = 0;
            qCurrentCategory++;
            qCurrentCategory = qCurrentCategory % qSpawnables.childCount;
        }
        HighlightQuickSpawnable();
    }
    public void ScrollUpQuickList()
    {
        if (!quickMenu.selectionBlock.activeSelf)
        {
            quickMenu.ShowSelections();
        }
        qCurrentSpawnable--;
        if (qCurrentSpawnable < 0)
        {
            qCurrentCategory--;
            qCurrentCategory = (qCurrentCategory + qSpawnables.childCount) % qSpawnables.childCount;
            qCurrentSpawnable = qSpawnables.GetChild(qCurrentCategory).childCount - 1;
        }
        HighlightQuickSpawnable();

    }

    void HighlightQuickSpawnable()
    {
        for(int loopOuter = 0; loopOuter < qSpawnables.childCount; loopOuter++)
        {
            for (int loopInner = 0; loopInner < qSpawnables.GetChild(loopOuter).childCount; loopInner++)
            {
                qSpawnables.GetChild(loopOuter).GetChild(loopInner).GetComponent<Text>().color = Color.white;
            }

        }
        qSpawnables.GetChild(qCurrentCategory).GetChild(qCurrentSpawnable).GetComponent<Text>().color = highlightedColor;

        quickMenu.StartCondenseListCooldown(qCurrentCategory == 0 ? "Offence" : qCurrentCategory == 1 ?
            "Defence" : "Decoration", qSpawnables.GetChild(qCurrentCategory).GetChild(qCurrentSpawnable).name);
    }

    public void QuickConstruct()
    {
        SpawnableType type = qCurrentCategory == 0 ? SpawnableType.Offence : qCurrentCategory == 1 ? 
            SpawnableType.Defence : SpawnableType.Decoration;
        string spawnableName = qSpawnables.GetChild(qCurrentCategory).GetChild(qCurrentSpawnable).name;
        if (spawnableName == "FloatingPlatform")
        {
            PlatformSelected();
        }
        else
        {
            ConstructionSelected(type, qSpawnables.GetChild(qCurrentCategory).GetChild(qCurrentSpawnable).name);
        }
    }
}
