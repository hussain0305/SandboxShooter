using UnityEngine.UI;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [Header("Objects for Function")]
    public Transform allCategories;
    public Transform[] allDetails;

    [Header("Objects for Appearance")]
    public Button[] allCategoryButtons;

    [Header("Materials")]
    public Material highlightedCategory;
    public Material regularCategory;

    public void Start()
    {
        highlightedCategory = new Material(highlightedCategory);
        regularCategory = new Material(regularCategory);
    }

    public void CategorySelected(int category)
    {
        foreach(Transform currCategory in allCategories)
        {
            currCategory.gameObject.SetActive(false);
            allCategoryButtons[currCategory.GetSiblingIndex()].GetComponent<Image>().material = regularCategory;

            if (currCategory.GetSiblingIndex() == category)
            {
                currCategory.gameObject.SetActive(true);
                allCategoryButtons[currCategory.GetSiblingIndex()].GetComponent<Image>().material = highlightedCategory;
            }
        }
    }

    public void SpawnableSelected(int categoryAndSpawnable)
    {
        int category = categoryAndSpawnable / 10;
        int spawnable = categoryAndSpawnable % 10;

        //if either of this goes into double digits, write proper algo above

        foreach (Transform currSpawnable in allDetails[category])
        {
            currSpawnable.gameObject.SetActive(false);
            if (currSpawnable.GetSiblingIndex() == spawnable)
            {
                currSpawnable.gameObject.SetActive(true);
            }
        }
    }

    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


}
