using UnityEngine.UI;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Mouse")]
    public Text mouseSensitivityText;
    public Slider mouseSensitivitySlider;

    public void PressedBack()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void OnEnable()
    {
        ShowCorrectMouseSensitivityValue();
    }

    #region Mouse

    public void MouseSensitivityAdjusted(float senstivity)
    {
        PlayerPrefs.SetInt("MouseSensitivity", (int)senstivity);
        mouseSensitivityText.text = "" + senstivity;

        if (GameObject.FindObjectOfType<GameManager>())
        {
            GameObject.FindObjectOfType<GameManager>().FetchLocalPlayer().mouseSensitivity = 4 * senstivity;
        }
    }

    public void ShowCorrectMouseSensitivityValue()
    {
        mouseSensitivityText.text = "" + PlayerPrefs.GetInt("MouseSensitivity", 50);
        mouseSensitivitySlider.value = PlayerPrefs.GetInt("MouseSensitivity", 50);
    }

    #endregion
}
