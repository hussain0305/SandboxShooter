using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    public bool changeTextAlso = true;
    public Color hoveredTextColor;
    public Material hoveredMaterial;

    private Color regularTextColor;
    private Material regularMaterial;

    void Start()
    {
        if (GetComponentInChildren<Text>())
        {
            regularTextColor = GetComponentInChildren<Text>().color;
        }
        regularMaterial = GetComponent<Image>().material;
        hoveredMaterial = new Material(hoveredMaterial);
    }

    public void Hovered()
    {
        GetComponent<Image>().material = hoveredMaterial;

        if (changeTextAlso && GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = hoveredTextColor;
        }

    }

    public void HoverLeft()
    {
        GetComponent<Image>().material = regularMaterial;
        
        if (changeTextAlso && GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = regularTextColor;
        }
    }
}
