using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }

    public Rail rail;
    public Transform lookAt;
    public float moveSpeed = 5.0f;

    public float zoomSpeed = 5f;
  

    private float previousDistance;

    private Vector3 lastPosition;

    private Camera myCamera;

    public Transform firstLimitPosition;
    public Transform lastLimitPosition;

    public float limitXRotation = 50f;

    private float initYRotation;
    private float initZRotation;

    private bool shouldUpdate;
    private bool transitioning;
    public float timeToTransition;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        myCamera = GetComponent<Camera>();
 
        lastPosition = transform.position;
        previousDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;

        initYRotation = transform.eulerAngles.y;
        initZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        HandlePosition();
        HandleZoomOnPlayer();
        HandlePlayerOnCamera();
    }

    private void HandlePosition()
    {
        //Set the limits of the camera and move the camera through the rails based on the position of the player
        if (transitioning) return;

        lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
        lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);
        transform.position = lastPosition;
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

        float targetDistance = (myCamera.transform.position - lookAt.position).sqrMagnitude;
        float distanceIncrement = (targetDistance - previousDistance);
        previousDistance = targetDistance;

        myCamera.fieldOfView -= distanceIncrement * zoomSpeed * Time.deltaTime;
        myCamera.fieldOfView = Mathf.Clamp(myCamera.fieldOfView, 40f, 50f);

    }

   
    public void ChangeLimits(Transform fp, Transform lp)
    {
        transitioning = true;
        firstLimitPosition = fp == null ? firstLimitPosition : fp;
        lastLimitPosition = lp == null ? lastLimitPosition : lp;
       
        StartCoroutine(TransitionLerp());
    }

    IEnumerator TransitionLerp()
    {
        float timer = 0f;
        Vector3 initialPos = myCamera.transform.position;

        while (timer < timeToTransition)
        {

            lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
            lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);

            transform.position = Vector3.Lerp(initialPos, lastPosition, timer / timeToTransition);
            timer += Time.deltaTime;

            yield return null;

        }
        transitioning = false;
    }

}



