using UnityEngine.UI;
using UnityEngine;
using System;

public class MapSelection : MonoBehaviour
{
    public Text mapName;

    private UIPanelSlider slider;
    private bool isOut;
    private EPhotonRoom room;

    private void Start()
    {
        room = GameObject.FindObjectOfType<EPhotonRoom>();
        slider = GetComponent<UIPanelSlider>();
        isOut = true;

        room.MapSelected(0);
    }
    public void MapSelected(string noAndName)
    {
        room.MapSelected(Int32.Parse(noAndName.Substring(0, 1)));
        mapName.text = noAndName.Substring(1);
        ToggleMapScreen();
    }

    public void ToggleMapScreen()
    {
        if (isOut)
        {
            slider.MoveIn();
        }
        else
        {
            slider.MoveOut();
        }
        isOut = !isOut;
    }
}
