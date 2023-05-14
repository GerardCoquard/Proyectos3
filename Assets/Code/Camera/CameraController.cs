using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }

    public Rail bottomRail;
    public Rail topRail;
    private Rail currentRail;

    public Transform lookAt;
    public Transform maxYLevel;
    public float moveSpeed = 5.0f;

    public float zoomSpeed = 5f;

    public float nodeZLimit;

    public float desiredDistance = 10f;
    private Vector3 lastPosition;


    private Camera myCamera;

    public Transform firstLimitPosition;
    public Transform lastLimitPosition;

    public float maxXrotation = 50f;
    public float minXRotation = -90f;

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
        currentRail = bottomRail;
        lastPosition = transform.position;
    }

    private void Update()
    {
        HandlePosition();
        HandlePlayerOnCamera();
        HandleCurrentRail();
    }

    private void HandleCurrentRail()
    {

        if (!topRail) return;
        if (lookAt.position.y > maxYLevel.position.y)
        {
            currentRail = topRail;
        }
        else
        {
            currentRail = bottomRail;
        }
    }
    private void HandlePosition()
    {
        //Set the limits of the camera and move the camera through the rails based on the position of the player
        if (transitioning) return;

        float desiredPosZ = lookAt.position.z - desiredDistance;

        lastPosition.y = currentRail.ProjectPositionOnRail(lookAt.position).y;
        lastPosition.x = currentRail.ProjectPositionOnRail(lookAt.position).x;

        //lastPosition.x = Mathf.Clamp(lastPosition.x, firstLimitPosition.position.x, lastLimitPosition.position.x);
        lastPosition.z = desiredPosZ;
        lastPosition.z = Mathf.Clamp(lastPosition.z, nodeZLimit, lastPosition.z);


        transform.position = Vector3.Lerp(transform.position, lastPosition, Time.deltaTime * moveSpeed);
    }
    private void HandlePlayerOnCamera()
    {

        Quaternion targetRotation = Quaternion.LookRotation(lookAt.transform.position - transform.position);
        Vector3 rotationInDegrees = targetRotation.eulerAngles;
        rotationInDegrees.x = ClampAngle(rotationInDegrees.x, minXRotation, maxXrotation);
        targetRotation = Quaternion.Euler(rotationInDegrees);


        Quaternion lerpedTarget = Quaternion.Slerp(myCamera.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        Quaternion LookYAxis = Quaternion.Euler(lerpedTarget.eulerAngles.x, myCamera.transform.rotation.eulerAngles.y, myCamera.transform.rotation.eulerAngles.z);
        myCamera.transform.rotation = LookYAxis;
        

    }

    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
    public void ChangeLimits(Transform fp, Transform lp, float zLimit)
    {
        firstLimitPosition = fp;
        lastLimitPosition = lp;
        nodeZLimit = zLimit;

        StartCoroutine(TransitionLerp());
    }

    public void ChangeRails(Rail newRail, Rail newAuxiliar, Transform yReference)
    {
        topRail = newAuxiliar;
        bottomRail = newRail;
        maxYLevel = yReference;
        currentRail = bottomRail;
    }
    IEnumerator TransitionLerp()
    {
        float timer = 0f;
        transitioning = true;
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





