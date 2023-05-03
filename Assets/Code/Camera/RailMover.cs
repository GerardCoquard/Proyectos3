using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Rail rail;
    public Transform lookAt;
    public float moveSpeed = 5.0f;
    public float interactFOV = 52f;
    public float zoomSpeed = 5f;
    public float maxDistanceToZoom = 310f;
    public float maxZoom = 41f;
    public float distanceFactor = 10f;
    public float fovReductionFactor = 0.1f;

    private float previousDistance;

    private Transform myTransform;
    private Vector3 lastPosition;

    private Camera myCamera;

    public Transform firstLimitRotation;
    public Transform lastLimitRotation;

    public Transform firstLimitPosition;
    public Transform lastLimitPosition;

    public float limitXRotation = 50f;

    private float initYRotation;
    private float initZRotation;

    public bool camFreezeYZRotation;

    private bool shouldUpdate;
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        myTransform = transform;
        lastPosition = myTransform.position;
        previousDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;

        initYRotation = transform.eulerAngles.y;
        initZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        HandlePosition();
        HandleZoomOnPlayer();
        LimitCamera();
        HandlePlayerOnCamera();
    }

    private void HandlePosition()
    {
        lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
        lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);
        myTransform.position = lastPosition;
    }
    private void HandlePlayerOnCamera()
    {

        Quaternion targetRotation = Quaternion.LookRotation(lookAt.transform.position - transform.position);
        Vector3 rotationInDegrees = targetRotation.eulerAngles;
        rotationInDegrees.x = Mathf.Clamp(rotationInDegrees.x, 0, limitXRotation);
        targetRotation = Quaternion.Euler(rotationInDegrees);

        if (camFreezeYZRotation)
        {

            targetRotation.y = initYRotation;
            targetRotation.z = initZRotation;
        }
        if (shouldUpdate)
        {
            myCamera.transform.rotation = Quaternion.Slerp(myCamera.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion lerpedTarget = Quaternion.Slerp(myCamera.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            Quaternion LookYAxis = Quaternion.Euler(lerpedTarget.eulerAngles.x, myCamera.transform.rotation.eulerAngles.y, myCamera.transform.rotation.eulerAngles.z);
            myCamera.transform.rotation = LookYAxis;
        }

    }

    private void HandleZoomOnPlayer()
    {

        float targetDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;
        float distanceIncrement = (targetDistance - previousDistance);
        previousDistance = targetDistance;

        myCamera.fieldOfView -= distanceIncrement * zoomSpeed * Time.deltaTime;
        myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, 40f, 50f);

    }

    private void LimitCamera()
    {

        Vector3 f = myCamera.WorldToViewportPoint(firstLimitRotation.position);
        Vector3 l = myCamera.WorldToViewportPoint(lastLimitRotation.position);

        if ((f.x >= 0 && f.x <= 1) && (f.y >= 0 && f.y <= 1) || (l.x >= 0 && l.x <= 1) && (l.y >= 0 && l.y <= 1))
        {
            shouldUpdate = false;

        }
        else
        {
            shouldUpdate = true;
        }
    }

}



