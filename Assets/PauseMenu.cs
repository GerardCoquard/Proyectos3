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

    public override void OnEnable()
    {
        base.OnEnable();
        PlayerController.instance.GetAnimator().SetBool("Paused", true);
        PlayerController.instance.isPaused = true;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerController.instance.GetAnimator().SetBool("Paused", false);
        PlayerController.instance.UnsetPause();
    }
}
