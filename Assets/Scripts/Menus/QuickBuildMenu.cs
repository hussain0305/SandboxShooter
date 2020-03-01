using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class QuickBuildMenu : MonoBehaviour
{
    public GameObject selectionBlock;
    public GameObject selectedBlock;

    //public Text categoryName;
    public Text spawnableName;

    public Text mainHeading;


    public void ShowSelections()
    {
        selectedBlock.SetActive(false);
        selectionBlock.SetActive(true);
        mainHeading.text = "Quick Build Menu";
    }

    public void ShowSelected()
    {
        selectionBlock.SetActive(false);
        selectedBlock.SetActive(true);
        mainHeading.text = "Currently Building";
    }

    public void StartCondenseListCooldown(string cat, string spn)
    {
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            StartCoroutine(CondenseList(cat, spn));
        }
    }

    IEnumerator CondenseList(string cat, string spn)
    {
        //categoryName.text = cat;
        spawnableName.text = spn;
        yield return new WaitForSeconds(2);
        ShowSelected();
    }
}
