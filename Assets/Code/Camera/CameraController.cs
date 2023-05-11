using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }

    public Rail rail;
    public Rail auxiliarRail;
    private Rail currentRail;

    public Transform lookAt;
    public Transform maxYLevel;
    public float moveSpeed = 5.0f;

    public float zoomSpeed = 5f;

    public float minFOV;
    public float maxFOV;

    public float nodeZLimit;

    private float previousDistance;
    public float desiredDistance = 10f;
    private Vector3 lastPosition;


    private Camera myCamera;

    public Transform firstLimitPosition;
    public Transform lastLimitPosition;

    public float limitXRotation = 50f;


    private bool shouldUpdate;
    private bool transitioning;
    public float timeToTransition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        currentRail = rail;
        lastPosition = transform.position;
        previousDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;
    }

    private void Update()
    {
        HandlePosition();
        HandlePlayerOnCamera();
        HandleCurrentRail();
        HandleZoomOnPlayer();

    }

    private void HandleCurrentRail()
    {

        if (!auxiliarRail) return;
        if (lookAt.position.y > maxYLevel.position.y)
        {
            currentRail = auxiliarRail;
        }
        else
        {
            currentRail = rail;
        }
    }
    private void HandlePosition()
    {
        //Set the limits of the camera and move the camera through the rails based on the position of the player
        if (transitioning) return;

        float targetDistance = lookAt.position.z - myCamera.transform.position.z;
        float distanceIncrement = (targetDistance - previousDistance);
        previousDistance = targetDistance;

        Vector3 directionToTarget = (lookAt.position - transform.position).normalized;
        Vector3 desiredPos = lookAt.position -  directionToTarget * desiredDistance;

        lastPosition.y = currentRail.ProjectPositionOnRail(lookAt.position).y;
        lastPosition.x = currentRail.ProjectPositionOnRail(lookAt.position).x;

        lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);
        lastPosition.z = desiredPos.z;
        lastPosition.z = Mathf.Clamp(lastPosition.z, nodeZLimit, lastPosition.z);


        transform.position = Vector3.Lerp(transform.position, lastPosition, Time.deltaTime * moveSpeed);
    }
    private void HandlePlayerOnCamera()
    {

        Quaternion targetRotation = Quaternion.LookRotation(lookAt.transform.position - transform.position);
        Vector3 rotationInDegrees = targetRotation.eulerAngles;
        rotationInDegrees.x = Mathf.Clamp(rotationInDegrees.x, 0, limitXRotation);
        targetRotation = Quaternion.Euler(rotationInDegrees);

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


        



    }


    public void ChangeLimits(Transform fp, Transform lp)
    {
        transitioning = true;
        firstLimitPosition = fp == null ? firstLimitPosition : fp;
        lastLimitPosition = lp == null ? lastLimitPosition : lp;

        StartCoroutine(TransitionLerp());
    }

    public void ChangeRail(Rail newRail, Transform yReference)
    {
        auxiliarRail = newRail;
        maxYLevel = yReference;
    }
    IEnumerator TransitionLerp()
    {
        float timer = 0f;
        Vector3 initialPos = myCamera.transform.position;

        while (timer < timeToTransition)
        {

            lastPosition = Vector3.Lerp(lastPosition, currentRail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
            lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);

            transform.position = Vector3.Lerp(initialPos, lastPosition, timer / timeToTransition);
            timer += Time.deltaTime;

            yield return null;

        }
        transitioning = false;
    }
    public void ChangeFocus(Transform target)
    {
        lookAt = target;
    }

}





