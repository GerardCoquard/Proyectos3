using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public List<Collider> noRotate = new List<Collider>();
    private List<PusheableObject> pushObjects = new List<PusheableObject>();
    public Transform myPlatform;

    public float degreesToRotate;
    public float rotationTime = 10f;
    private bool isRotating;

    public void RotatePlatformRight()
    {
        if (!isRotating)
        {

            StartCoroutine(RotateObject(myPlatform.rotation, myPlatform.rotation * Quaternion.Euler(0, degreesToRotate, 0), rotationTime));
        }
    }
    public void RotatePlatformLeft()
    {
        if (!isRotating)
        {

            StartCoroutine(RotateObject(myPlatform.rotation, myPlatform.rotation * Quaternion.Euler(0, -degreesToRotate, 0), rotationTime));
        }
    }

    IEnumerator RotateObject(Quaternion startRotation, Quaternion endRotation, float duration)
    {
        float t = 0f;
        isRotating = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            myPlatform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        isRotating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(noRotate.Contains(other)) return;
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pusheableObject != null) 
        {
            pusheableObject.transform.SetParent(myPlatform);
            pushObjects.Add(pusheableObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pushObjects.Contains(pusheableObject))
        {
            pusheableObject.transform.SetParent(null);
            pushObjects.Remove(other.GetComponentInParent<PusheableObject>());
        }
    }
}
