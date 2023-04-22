using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InterfacesManager : MonoBehaviour
{
    [Serializable]
    public struct _interface
    {
        public string actionName;
        public Menu interfaceObject;
    }
    public _interface[] _interfaces;
    private void OnEnable() {
        foreach (_interface _interface in _interfaces)
        {
            InputManager.GetAction(_interface.actionName).action += ((GameObject) => Open(_interface.interfaceObject));
        }
    }
    private void OnDisable() {
        foreach (_interface _interface in _interfaces)
        {
            InputManager.GetAction(_interface.actionName).action -= ((GameObject) => Open(_interface.interfaceObject));
        }
    }
    void Open(Menu interfaceToOpen)
    {
        interfaceToOpen.OpenMenu();
    }
}
