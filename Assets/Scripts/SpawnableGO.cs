using UnityEngine;

public class SpawnableGO : MonoBehaviour
{
    [HideInInspector]
    public bool isUsable;
    [HideInInspector]
    public string displayName;


    [Header("Spawnable Properties")]
    public SpawnableType type;
    public KindOfSpawnable kindOfSpawnable;
    public float buildTime;
    
    //Components
    private SpawnableHealth healthComponent;
    private SpawnableAppearance appearanceComponent;

    public void Start()
    {
        isUsable = false;
        //AssignLayer();

        //Establish references for all components
        FetchAppearanceComponent();
        FetchHealthComponent();
    }

    public void AssignLayer()
    {
        int layer = LayerMask.NameToLayer("SpawnableGO");
        gameObject.layer = layer;
        foreach (Transform curr in transform)
        {
            curr.gameObject.layer = layer;
        }
    }

    public void SpawnableIsReadyForBusiness()
    {
        if (healthComponent)
        {
            healthComponent.enabled = true;
        }
        isUsable = true;
    }


    #region Health
    public void FetchHealthComponent()
    {
        healthComponent = GetComponent<SpawnableHealth>();
        if (healthComponent)
        {
            healthComponent.enabled = false;
        }
    }
    #endregion

    #region Appearance
    public void FetchAppearanceComponent()
    {
        appearanceComponent = GetComponentInChildren<SpawnableAppearance>();
    }
    #endregion

    #region Getters and Setters
    public SpawnableAppearance GetAppearanceComponent()
    {
        return appearanceComponent;
    }
    public SpawnableHealth GetHealthComponent()
    {
        return healthComponent;
    }
    
    #endregion
}
