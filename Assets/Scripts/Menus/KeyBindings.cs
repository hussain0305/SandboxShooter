using UnityEngine.UI;
using UnityEngine;

public class KeyBindings : MonoBehaviour
{
    public UIPanelSlider slider;
    public Text keyBindingButtonText;
    
    [HideInInspector]
    public bool isOut;

    private void Start()
    {
        isOut = true;
    }
    public void KeyBindingsButtonPressed()
    {
        ToggleKeyBindingsPanel();
    }

    public void ToggleKeyBindingsPanel()
    {
        if (isOut)
        {
            slider.MoveIn();
            keyBindingButtonText.color = Color.green;
        }
        else
        {
            slider.MoveOut();
            keyBindingButtonText.color = Color.white;
        }
        isOut = !isOut;
    }
}
