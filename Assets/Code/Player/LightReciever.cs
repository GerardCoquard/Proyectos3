using System;
using UnityEngine;
using UnityEngine.Events;

public class LightReciever : MonoBehaviour
{
    public UnityEvent OnLightRecived;
    [SerializeField] private bool lightGoesThrough;
    private bool isTriggered;
    public void DoAction()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            OnLightRecived?.Invoke();
            if (lightGoesThrough)
            {
                gameObject.layer = 10;
            }
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

    public bool GetLightGoesThrough()
    {
        return lightGoesThrough;
    }
}
