using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_MainMainMenu : MonoBehaviour
{
    public GameObject journal;

    public void Start()
    {
        Cursor.visible = true;
    }
    public void ButtonPressed_PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ButtonPressed_Journal()
    {
        journal.SetActive(!journal.activeSelf);
    }

    public void ButtonPressed_Settings()
    {

    }

    public void ButtonPressed_Exit()
    {
        Application.Quit();
    }
}
