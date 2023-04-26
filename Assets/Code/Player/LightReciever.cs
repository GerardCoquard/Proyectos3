using System;
using UnityEngine;
using UnityEngine.Events;

public class LightReciever : MonoBehaviour
{
    public UnityEvent OnLightRecived;
    private bool isTriggered;

    public void DoAction()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            OnLightRecived?.Invoke();
        }
    }

    public void DebugAction()
    {
        Debug.Log("DEBUG ACTION TEST");
    }

    public void SetIsHitted(bool state)
    {
        isTriggered = state;
    }

    public bool GetIsHitted()
    {
        return isTriggered;
    }
}
