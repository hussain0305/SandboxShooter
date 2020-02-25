using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Eurus/Spawnable")]
public class Spawnable : ScriptableObject
{
    public SpawnableType type;
    public new string name;
    public string displayName;
    public Sprite image;
    public string description;
    public int health;
    public int constructionEnergyRequired;
    public GameObject prefab;
    public bool isGridAligned;

    public string[] pathStrings;
}
