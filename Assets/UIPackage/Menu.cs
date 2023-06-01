using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public GameObject firstButton;
    public bool rememberLastButton = true;
    public bool cursorLock = false;
    public bool freezeTime = true;
    public bool closeOnBack = true;
    bool initialized = false;
    public UnityEvent onBack;
    string previousActionMap = "";
    [NonSerialized] public GameObject lastButton;
    public virtual void OnStart(){initialized = true;}
    public virtual void OnEnable()
    {
        if(!initialized) OnStart();
        Cursor.visible = true;
        HighlightButton();
        SubscribeOnBack();
        FreezeTime();
        LockCursor();
        InputManager.ChangeActionMap("UI");
    }
    public virtual void OnDisable() {
        if(previousActionMap!="") InputManager.ChangeActionMap(previousActionMap);
        UnsubscribeOnBack();
        UnlockCursor();
    }
    public void HighlightButton()
    {
        if(InputManager.device == Devices.Gamepad) UIUtilities.HighlightButton(lastButton==null || !rememberLastButton? firstButton : lastButton);
    }
    void FreezeTime()
    {
        if(freezeTime) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
    void LockCursor()
    {
        if(cursorLock) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.None;
    }
    void UnlockCursor()
    {
        if(OptionsManager.cursorLock) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.None;
    }
    public void Close(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            gameObject.SetActive(false);
        }
    }
    public void SubscribeOnBack()
    {
        if(closeOnBack) InputManager.GetAction("Back").action += Close;
        InputManager.GetAction("Back").action += OnBack;
    }
    public void UnsubscribeOnBack()
    {
        if(closeOnBack) InputManager.GetAction("Back").action -= Close;
        InputManager.GetAction("Back").action -= OnBack;
    }
    void OnBack(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton()) onBack?.Invoke();
    }
    public void SwitchActionMap(string actionMap)
    {
        InputManager.ChangeActionMap(actionMap);
    }
    public void OpenMenu()
    {
        previousActionMap = InputManager.GetCurrentActionMap();
        gameObject.SetActive(true);
    }
    public void SubscribeOnBackDelayed()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        SubscribeOnBack();
    }

}
