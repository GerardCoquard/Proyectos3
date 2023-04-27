using System;
using UnityEngine;
using UnityEngine.Events;

public class LightReciever : MonoBehaviour
{
    public UnityEvent OnLightRecived;
    public UnityEvent OnLightNotRecived;
    public bool lightGoesThrough;
    private bool isTriggered;
    public void DoAction()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            OnLightRecived?.Invoke();           
        }
    }

    private void Update()
    {
        if (!isTriggered) gameObject.layer = 0;
    }

    public void UndoAction()
    {

        OnLightNotRecived?.Invoke();
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
