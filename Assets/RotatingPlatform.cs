using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public List<Collider> noRotate = new List<Collider>();
    private List<PusheableObject> pushObjects = new List<PusheableObject>();

    public float degreesToRotate;
    public float rotationTime = 10f;
    private bool isRotating;

    public void RotatePlatformRight()
    {
        if (!isRotating)
        {

            StartCoroutine(RotateObject(transform.rotation, transform.rotation * Quaternion.Euler(0, degreesToRotate, 0), rotationTime));
        }
    }
    public void RotatePlatformLeft()
    {
        if (!isRotating)
        {

            StartCoroutine(RotateObject(transform.rotation, transform.rotation * Quaternion.Euler(0, -degreesToRotate, 0), rotationTime));
        }
    }

    IEnumerator RotateObject(Quaternion startRotation, Quaternion endRotation, float duration)
    {
        for (int i = 0; i < pushObjects.Count; i++)
        {
            if(pushObjects[i] == null)
            {
                pushObjects.RemoveAt(i);
                i++;
            }
            else pushObjects[i].rb.interpolation = RigidbodyInterpolation.None;
        }

        float t = 0f;
        isRotating = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        isRotating = false;
        for (int i = 0; i < pushObjects.Count; i++)
        {
            if(pushObjects[i] == null)
            {
                pushObjects.RemoveAt(i);
                i++;
            }
            else pushObjects[i].rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(noRotate.Contains(other)) return;
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pusheableObject != null) 
        {
            pusheableObject.transform.SetParent(transform);
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
