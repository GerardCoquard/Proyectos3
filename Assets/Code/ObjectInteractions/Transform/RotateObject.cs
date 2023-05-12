using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Quaternion initialRotation;
    Quaternion finalRotation;
    public float timeToReach;
    public Vector3 _finalRotation;
    float angleBetweenRotations;
    bool locked;

    private void Start()
    {
        initialRotation = transform.rotation;
        finalRotation = Quaternion.Euler(_finalRotation);
        angleBetweenRotations = Quaternion.Angle(initialRotation,finalRotation);
    }
    public void Rotate()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(RotateCoroutine());
    }
    public void ResetRotation()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(ResetCoroutine());
    }
    IEnumerator RotateCoroutine()
    {
        float angleToRotation =  Quaternion.Angle(transform.rotation, finalRotation);
        float time = angleToRotation / angleBetweenRotations * timeToReach;
        Quaternion _initRot = transform.rotation;
        float timer = 0f;
        while (timer<time)
        {
            transform.rotation = Quaternion.Lerp(_initRot,finalRotation,timer/time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = finalRotation;
    }
    IEnumerator ResetCoroutine()
    {
        float angleToRotation =  Quaternion.Angle(transform.rotation, initialRotation);
        float time = angleToRotation / angleBetweenRotations * timeToReach;
        Quaternion _initRot = transform.rotation;
        float timer = 0f;
        while (timer<time)
        {
            transform.rotation = Quaternion.Lerp(_initRot,initialRotation,timer/time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.rotation = initialRotation;
    }
    
    public void SetLocked(bool state)
    {
        locked = state;
    }
}
