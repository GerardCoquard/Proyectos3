using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableHandler : MonoBehaviour,ISelectHandler,IPointerEnterHandler,IPointerExitHandler,IDeselectHandler,IEndDragHandler,IBeginDragHandler,IDragHandler
{
    public bool clickSound = true;
    public string highlightSoundName = "ButtonHighlightSound";
    public string clickSoundName = "ButtonClickSound";
    public bool unselectOnClick = false;
    public bool dontUseFlyers;
    public UnityEvent onEnable;
    public UnityEvent onClick;
    public UnityEvent<bool> onClickBool;
    public UnityEvent<int> onClickInt;
    public UnityEvent onHighlight;
    public UnityEvent onHighlightGamepad;
    public UnityEvent onUnhighlight;
    public UnityEvent onBeginDrag;
    [NonSerialized] public bool interactable = true;
    Flyers flyer;
    private void Awake() {
        switch(GetComponent<Selectable>())
        {
            case Button button:
            button.onClick.AddListener(Click);
            break;
            case Toggle toggle:
            toggle.onValueChanged.AddListener(Click);
            break;
            case TMP_Dropdown dropdown:
            dropdown.onValueChanged.AddListener(Click);
            break;
            case Dropdown dropdown:
            dropdown.onValueChanged.AddListener(Click);
            break;
            default:
            break;
        }
        flyer = GetComponentInChildren<Flyers>();
    }
    private void OnEnable() {
        onEnable?.Invoke();
    }
    public void OnSelect(BaseEventData eventData)
    {
        if(InputManager.device == Devices.Gamepad) Hihghlight();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if(InputManager.device == Devices.Gamepad) Unhighlight();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(InputManager.device == Devices.Keyboard) Hihghlight();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(InputManager.device == Devices.Keyboard) Unhighlight();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(InputManager.device == Devices.Gamepad) return;
        Unhighlight();
        if(unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(InputManager.device == Devices.Gamepad) return;
        if(!interactable) return;
        Menu parentMenu = GetComponentInParent<Menu>();
        if(parentMenu!=null) parentMenu.lastButton = gameObject;
        if(unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
        onBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(InputManager.device == Devices.Gamepad) return;
        Unhighlight();
        if(unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
    }
    void Hihghlight()
    {
        AudioManager.Play(highlightSoundName);
        if(flyer!=null && !dontUseFlyers) flyer.SetFlyers(true);
        if(InputManager.device == Devices.Gamepad) onHighlightGamepad?.Invoke();
        onHighlight?.Invoke();
    }
    public void Unhighlight()
    {
        if(flyer!=null && !dontUseFlyers) flyer.SetFlyers(false);
        onUnhighlight?.Invoke();
    }
    void Click()
    {
        if(!interactable) return;
        if(clickSound) AudioManager.Play(clickSoundName);
        Menu parentMenu = GetComponentInParent<Menu>();
        if(parentMenu!=null) parentMenu.lastButton = gameObject;
        if(InputManager.device == Devices.Keyboard && unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
        onClick?.Invoke();
    }
    void Click(bool toggleValue)
    {
        if(!interactable) return;
        if(clickSound) AudioManager.Play(clickSoundName);
        Menu parentMenu = GetComponentInParent<Menu>();
        if(parentMenu!=null) parentMenu.lastButton = gameObject;
        if(InputManager.device == Devices.Keyboard && unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
        onClickBool?.Invoke(toggleValue);
    }
    void Click(int dropdownValue)
    {
        if(!interactable) return;
        if(clickSound) AudioManager.Play(clickSoundName);
        Menu parentMenu = GetComponentInParent<Menu>();
        if(parentMenu!=null) parentMenu.lastButton = gameObject;
        if(InputManager.device == Devices.Keyboard && unselectOnClick) EventSystem.current.SetSelectedGameObject(null);
        onClickInt?.Invoke(dropdownValue);
    }
}
