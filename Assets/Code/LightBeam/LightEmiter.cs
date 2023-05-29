using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightEmiter : MonoBehaviour
{
    LightBeam beam;
    public Transform rayStartPos;
    public float width;
    public float growthSpeed;
    public Material material;
    public int maxBounces;
    public RayColor rayColor = RayColor.Anyone;
    public bool active = true;
    public ColorPropertySetter colorPropertySetter;
    public float offIntensity;
    public float onIntensity;
    public float fadeOutSpeed;
    public bool inverted;

    private void Start()
    {
        beam = new LightBeam(transform.position, inverted ? -transform.forward : transform.forward, material, Physics.AllLayers,maxBounces,rayColor,width, transform,growthSpeed);
        SetPower(active);
    }

    private void Update()
    {
        if(active) beam.ExecuteRay(rayStartPos.position, inverted ? -rayStartPos.forward : rayStartPos.forward, beam.lineRenderer);
        else
        {
            beam.ResetBeam();
        }
    }
    public void SetPower(bool state)
    {
        active = state;
        if(colorPropertySetter!=null) colorPropertySetter.SetIntensity(material,active?onIntensity:offIntensity);
    }
    

}