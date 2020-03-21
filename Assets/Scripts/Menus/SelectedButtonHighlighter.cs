using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedButtonHighlighter : MonoBehaviour
{
    private int currentHighlighted;

    void OnEnable()
    {
        foreach (Transform currButton in transform)
        {
            currButton.GetComponent<ButtonHover>().ResetButton();
        }
    }

    public void ButtonSelected(int index)
    {
        transform.GetChild(currentHighlighted).GetComponent<ButtonHover>().ResetButton();
        currentHighlighted = index;
    }
}
