using System;
using UnityEngine;
using UnityEngine.Events;

public class LightReciever : MonoBehaviour
{
    public UnityEvent OnLightRecived;
    public UnityEvent OnLightNotRecived;
    public bool lightGoesThrough;
    LightBeam extraLightBeam;
    [NonSerialized] public Vector3 pos;
    [NonSerialized] public Vector3 dir;
    public void DoAction(LightBeam beam)
    {
        if(lightGoesThrough)
        {
            extraLightBeam =  new LightBeam(beam);
        }
        OnLightRecived?.Invoke();           
    }
    public void UpdatePoint(Vector3 _pos,Vector3 _dir)
    {
        pos = _pos;
        dir = _dir;
    }
    public void UndoAction()
    {
        if(lightGoesThrough)
        {
            Destroy(extraLightBeam.lightGameObject);
            extraLightBeam = null;
        }
        OnLightNotRecived?.Invoke();
    }
    private void LateUpdate() {
        if(extraLightBeam!=null) extraLightBeam.ExecuteRay(pos,dir,extraLightBeam.lineRenderer);
    }
}
