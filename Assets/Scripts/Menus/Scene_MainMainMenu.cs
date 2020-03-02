using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_MainMainMenu : MonoBehaviour
{
    public GameObject howToPlay;
    public GameObject journal;
    public GameObject settings;

    public void Start()
    {
        Cursor.visible = true;
    }
    public void ButtonPressed_PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonPressed_HowToPlay()
    {
        howToPlay.SetActive(!howToPlay.activeSelf);
    }

    public void ButtonPressed_Journal()
    {
        journal.SetActive(!journal.activeSelf);
    }

    public void ButtonPressed_Settings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void ButtonPressed_Exit()
    {
        Application.Quit();
    }
}
