using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public string sceneName;
    public string creditsSceneName;
    public override void OnEnable()
    {
        if(DataManager.Load<int>("roomID") != 0)
        {
            firstButton = continueButton;
        }
        else
        {
            continueButton.SetActive(false);
            firstButton = newGameButton;
        }
        base.OnEnable();
    }
    public void NewGame()
    {
        DataManager.Save("roomID",0);
        Continue();
    }
    public void Continue()
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
