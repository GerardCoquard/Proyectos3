using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : Menu
{
    AudioSourceHandler sound;
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if(PlayerController.instance == null) return;
        InputManager.GetAction("Unpause").action += CloseMenu;
        PlayerController.instance.GetAnimator().SetBool("Paused", true);
        PlayerController.instance.isPaused = true;
        sound = AudioManager.Play("openMenu").DontDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if(PlayerController.instance == null) return;
        InputManager.GetAction("Unpause").action -= CloseMenu;
        PlayerController.instance.GetAnimator().SetBool("Paused", false);
        PlayerController.instance.UnsetPause();
        sound.FadeOut(2);
        AudioManager.Play("closeMenu").Pitch(1+Random.Range(-0.4f,-0.2f)).FadeOut(1.5f);
    }
    public void CloseMenu(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            gameObject.SetActive(false);
        }
    }
}
