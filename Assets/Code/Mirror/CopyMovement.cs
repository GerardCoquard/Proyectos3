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
        UpdatePosition();
    }

    private void SetElementInMirrorPosition()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 90f);

    }


    private void UpdatePosition()
    {

        float distanceToReference = Mathf.Abs(MirrorPuzzleManager.instance.planeReference.position.z - elementToCopy.position.z);
        Vector3 newPosition = Vector3.zero;

        newPosition.x = elementToCopy.position.x;
        newPosition.z = (elementToCopy.position.z + (2 * distanceToReference));
        newPosition.y = elementToCopy.position.y;

        transform.position = newPosition;
    }

}
