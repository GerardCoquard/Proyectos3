using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class UIUtilities
{
    static UIUtilities()
    {
        InputManager.OnDeviceChanged += DeviceChanged;
    }
    public static void Init(){}
    public static void HighlightButton(GameObject button)
    {
        if(InputManager.device == Devices.Gamepad) CoroutineManager.StartCoroutine(HighlightButtonCoroutine(button));
        else EventSystem.current.SetSelectedGameObject(null);
    }
    public static IEnumerator HighlightButtonCoroutine(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(button);
    }
    static void DeviceChanged(Devices newDevice)
    {
        switch (newDevice)
        {
            case Devices.Keyboard:
            EventSystem.current.SetSelectedGameObject(null);
            break;

            case Devices.Gamepad:
            Selectable selectable = GetTopLeftSelectable();
            selectable?.Select();
            selectable?.OnSelect(null);
            break;
        }
    }
    public static Selectable GetTopLeftSelectable()
    {
        var listOfSelectables = Selectable.allSelectablesArray;
        if(listOfSelectables.Length > 0)
        {
            Selectable targetButton = listOfSelectables[0];
            RectTransform targetRectTrans = targetButton.GetComponent<RectTransform>();

            foreach (Selectable selectableUI in listOfSelectables)
            {
                RectTransform rectTrans = selectableUI.GetComponent<RectTransform>();
                if(rectTrans.position.y < targetRectTrans.position.y)
                {
                    continue;
                }
                if(rectTrans.position.y > targetRectTrans.position.y)
                {
                    targetButton = selectableUI;
                    targetRectTrans = rectTrans;
                    continue;
                }
                if(rectTrans.position.x < targetRectTrans.position.x)
                {
                    targetButton = selectableUI;
                    targetRectTrans = rectTrans;
                }
            }
            return targetButton;
        }
        return null;
    }
}
