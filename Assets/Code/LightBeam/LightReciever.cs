using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class LightReciever : MonoBehaviour
{
    public UnityEvent OnLightRecived;
    public UnityEvent OnLightNotRecived;
    public bool lightGoesThrough;
    public RayColor currentColor = RayColor.Anyone;
    public Material linkedBeamMaterial;
    public ColorPropertySetter colorPropertySetter;
    public float offIntensity;
    public float onIntensity;
    Dictionary<LightBeam,LightBeamData> crossingBeams = new Dictionary<LightBeam, LightBeamData>();
    private void Start() {
        OnLightRecived.AddListener(() => colorPropertySetter.SetIntensity(linkedBeamMaterial,onIntensity));
        OnLightNotRecived.AddListener(() => colorPropertySetter.SetIntensity(linkedBeamMaterial,offIntensity));
        colorPropertySetter.SetIntensity(linkedBeamMaterial,offIntensity);
    }
    public void DoAction(LightBeam beam)
    {
        if(beam.rayType != currentColor && currentColor != RayColor.Anyone) return;
        if(crossingBeams.Count == 0) OnLightRecived?.Invoke();
        if(lightGoesThrough) StartCoroutine(AddBeam(beam));
        else crossingBeams.Add(beam,new LightBeamData(null,Vector3.zero,Vector3.zero));
    }
    public void UpdatePoint(LightBeam beam,Vector3 _pos,Vector3 _dir)
    {
        if(beam.rayType != currentColor && currentColor != RayColor.Anyone) return;
        if(!lightGoesThrough) return;
        foreach (KeyValuePair<LightBeam, LightBeamData> entry in crossingBeams)
        {
            if(entry.Key == beam)
            {
                entry.Value.pos = _pos;
                entry.Value.dir = _dir;
            }
        }
    }
    public void UndoAction(LightBeam beam)
    {
        if(beam.rayType != currentColor && currentColor != RayColor.Anyone) return;
        if(lightGoesThrough) CheckChildBeams(beam);
        else
        {
            crossingBeams.Remove(beam);
            if(crossingBeams.Count == 0) OnLightNotRecived?.Invoke();
        }
    }
    private void LateUpdate() {
        if(!lightGoesThrough) return;
        foreach (KeyValuePair<LightBeam, LightBeamData> entry in crossingBeams)
        {
            entry.Value.beam.ExecuteRay(entry.Value.pos,entry.Value.dir,entry.Value.beam.lineRenderer);
        }
    }
    void CheckChildBeams(LightBeam beam)
    {
        if(crossingBeams.ContainsKey(beam))
        {
            CheckChildBeams(crossingBeams[beam].beam);
            StartCoroutine(DestroyBeamChild(beam));
        }
    }
    IEnumerator DestroyBeamChild(LightBeam beam)
    {
        yield return new WaitForEndOfFrame();
        GameObject beamToDestroy= crossingBeams[beam].beam.lightGameObject;
        crossingBeams.Remove(beam);
        Destroy(beamToDestroy);
        if(crossingBeams.Count == 0) OnLightNotRecived?.Invoke();
    }
    IEnumerator AddBeam(LightBeam beam)
    {
        yield return new WaitForEndOfFrame();
        LightBeam extraLightBeam =  new LightBeam(beam);
        LightBeamData extraData = new LightBeamData(extraLightBeam,Vector3.zero,Vector3.zero);
        crossingBeams.Add(beam,extraData);
    }
}
