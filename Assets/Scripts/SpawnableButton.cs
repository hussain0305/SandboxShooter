using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnableButton : MonoBehaviour
{
    public SpawnableType type;
    public string id;

    private ConstructionMenu construction;
    private UIInfoPopup createdPopup;
    private Spawnable correspodingSpawnable;
    // Start is called before the first frame update
    void Start()
    {
        SetupEverything();
    }

    public void SetupEverything()
    {
        construction = GetComponentInParent<ConstructionMenu>();
        correspodingSpawnable = GameObject.FindObjectOfType<GameManager>().GetBlueprint(type, id);
        createdPopup = construction.GetPopup();
    }

    // Update is called once per frame
    public void SpawnableButtonClicked()
    {
        ExitPointer();
        if (!construction)
        {
            return;
        }
        construction.ConstructionSelected(type, id);
    }

    public void FloatingPlatformButtonClicked()
    {
        ExitPointer();
        if (!construction)
        {
            return;
        }
        construction.PlatformSelected();
    }

    public void EnterPointer()
    {
        createdPopup.gameObject.SetActive(true);
        createdPopup.spawnableName.text = correspodingSpawnable.displayName;
        createdPopup.spawnableImage.sprite = correspodingSpawnable.image;
        createdPopup.description.text = correspodingSpawnable.description;
        createdPopup.healthValue.text = "" + correspodingSpawnable.health;
        //set attack
        //set dps
        //set dps
        createdPopup.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void ExitPointer()
    {
        createdPopup.gameObject.SetActive(false);
    }
}
