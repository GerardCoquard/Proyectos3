using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public string sceneName;
    public string creditsSceneName;
    public void NewGame()
    {
        Loader.instance.LoadScene(sceneName);
        InputManager.ChangeActionMap("Player");
        Time.timeScale = 1;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Credits()
    {
        Loader.instance.LoadScene(creditsSceneName);
        InputManager.ChangeActionMap("Player");
        Time.timeScale = 1;
    }
}
