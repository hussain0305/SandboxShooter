using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    [HideInInspector]
    public bool isSelected;

    public bool changeText = true;
    public bool changeMaterial = true;
    
    public Color hoveredTextColor;
    public Material hoveredMaterial;
    public Color selectedTextColor;
    public Material selectedMaterial;
    [Range(1.0f, 2.0f)]
    public float highlightedTextSizeMultiple = 1;

    private Color regularTextColor;
    private Material regularMaterial;
    private int regularTextSize;

    private SelectedButtonHighlighter centralHighlighter;

    private bool buttonInitialized = false;

    void Awake()
    {
        buttonInitialized = false;
        if (GetComponentInChildren<Text>())
        {
            regularTextColor = GetComponentInChildren<Text>().color;
            regularTextSize = GetComponentInChildren<Text>().fontSize;
        }
        if (GetComponentInChildren<Image>())
        {
            regularMaterial = GetComponentInChildren<Image>().material;
            if (hoveredMaterial)
            {
                hoveredMaterial = new Material(hoveredMaterial);
            }
            if (selectedMaterial)
            {
                selectedMaterial = new Material(selectedMaterial);
            }
        }

        isSelected = false;

        centralHighlighter = GetComponentInParent<SelectedButtonHighlighter>();
        buttonInitialized = true;
    }

    public void ResetButton()
    {
        if (!buttonInitialized)
        {
            return;
        }
        if (GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = regularTextColor;
            GetComponentInChildren<Text>().fontSize = regularTextSize;
        }
        if (GetComponentInChildren<Image>())
        {
            GetComponentInChildren<Image>().material = regularMaterial;
        }
        isSelected = false;
    }

    public void Hovered()
    {
        if (isSelected)
        {
            return;
        }

        if (changeMaterial)
        {
            GetComponentInChildren<Image>().material = hoveredMaterial;
        }

        if (changeText && GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = hoveredTextColor;
            GetComponentInChildren<Text>().fontSize = (int)(regularTextSize * highlightedTextSizeMultiple);
        }

    }

    public void HoverLeft()
    {
        if (isSelected)
        {
            return;
        }
        
        if (changeMaterial)
        {
            GetComponentInChildren<Image>().material = regularMaterial;
        }
        
        if (changeText && GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = regularTextColor;
            GetComponentInChildren<Text>().fontSize = regularTextSize;
        }
    }

    public void ButtonClicked()
    {
        centralHighlighter.ButtonSelected(transform.GetSiblingIndex());

        if (changeMaterial)
        {
            GetComponentInChildren<Image>().material = selectedMaterial;
        }

        if (changeText && GetComponentInChildren<Text>())
        {
            GetComponentInChildren<Text>().color = selectedTextColor;
            GetComponentInChildren<Text>().fontSize = (int)(regularTextSize * highlightedTextSizeMultiple);
        }

        isSelected = true;
    }
}
