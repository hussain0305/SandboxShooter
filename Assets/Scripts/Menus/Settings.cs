using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
