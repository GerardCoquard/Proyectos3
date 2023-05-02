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
    private float initialFOV;
    public float maxDistanceToZoom = 310f;
    public float farFOV = 41f;

    private Transform myTransform;
    private Vector3 lastPosition;
    private Quaternion initialRotation;

    private Camera myCamera;
    private PlayerController controller;

    public Transform firstLimit;
    public Transform lastLimit;

    private bool shouldUpdate;
    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        myCamera = GetComponent<Camera>();
        initialFOV = myCamera.fieldOfView;
        initialRotation = myCamera.transform.rotation;
        myTransform = transform;
        lastPosition = myTransform.position;
    }

    private void Update()
    {

        lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
        myTransform.position = lastPosition;



        HandleZoomOnPlayer();
        LimitCamera();
        HandlePlayerOnCamera();
    }

    private void HandlePlayerOnCamera()
    {
        //This should just affect the rotation in the Y axis
      
        Quaternion targetRotation = Quaternion.LookRotation(lookAt.transform.position - transform.position);
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
        //if controller is interacting zoom in
        //if not zoom out
        float targetDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;

        if (controller.isInteracting)
        {
            float delta = (interactFOV - myCamera.fieldOfView) * Time.deltaTime * zoomSpeed;
            myCamera.fieldOfView += delta;
        }
        else if(targetDistance >= maxDistanceToZoom)
        {
            float delta = (farFOV - myCamera.fieldOfView) * Time.deltaTime * zoomSpeed;
            myCamera.fieldOfView += delta;
        }
        else
        {
            float delta = (myCamera.fieldOfView - initialFOV) * Time.deltaTime * zoomSpeed;
            myCamera.fieldOfView -= delta;

            myCamera.transform.rotation = Quaternion.Slerp(myCamera.transform.rotation, initialRotation, moveSpeed * Time.deltaTime);
        }
    }

    private void LimitCamera()
    {
        //Get limit positions on viewport
        Vector3 f = myCamera.WorldToViewportPoint(firstLimit.position);
        Vector3 l = myCamera.WorldToViewportPoint(lastLimit.position);

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

