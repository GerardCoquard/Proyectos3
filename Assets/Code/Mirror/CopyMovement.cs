using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class CopyMovement : MonoBehaviour
{
    [SerializeField] private Transform elementToCopy;
    

    private void Start()
    {
        SetElementInMirrorPosition();
    }

    private void Update()
    {
        
    }
    
    private void SetElementInMirrorPosition()
    {
        transform.position = elementToCopy.position;
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 otherPosition = new Vector2(elementToCopy.position.x, elementToCopy.position.z);
        float distanceToReference = (otherPosition - myPosition).magnitude;

        transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.z) + ((distanceToReference * transform.forward) * 2);

    }

}
