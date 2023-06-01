using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
