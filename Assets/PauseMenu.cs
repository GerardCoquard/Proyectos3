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
        if(PlayerController.instance == null) return;
        PlayerController.instance.GetAnimator().SetBool("Paused", true);
        PlayerController.instance.isPaused = true;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if(PlayerController.instance == null) return;
        PlayerController.instance.GetAnimator().SetBool("Paused", false);
        PlayerController.instance.UnsetPause();
    }
}
