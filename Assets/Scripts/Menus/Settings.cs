using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public KeyBindings keyBindingScreen;
    public void OnEnable()
    {
        if (keyBindingScreen)
        {
            keyBindingScreen.slider.ResetPosition();
            keyBindingScreen.isOut = true;
            keyBindingScreen.keyBindingButtonText.color = Color.white;
        }
    }

    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
