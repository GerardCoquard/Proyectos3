using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }
    public RoomTrigger firstRoom;
    BoxCollider box;
    public float speed;
    public float height;
    public float depth;
    public float offset;
    float extraDepth;
    float extraHeight;
    float maxAngle;
    Transform target;
    float xMin;
    float xMax;
    float zMin;
    float zMax;
    //
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
        ChangeFocus(PlayerController.instance.transform);
        firstRoom.ChangeRoom();
    }

    private void Update()
    {
        HandlePosition();
        HandlePlayerOnCamera();
    }
    private void HandlePosition()
    {
        //Set the limits of the camera and move the camera through the rails based on the position of the player
        Vector3 targetPos = target.position + new Vector3(PlayerController.instance.GetDirection().x,0,PlayerController.instance.GetDirection().y) * offset;
        float xPos = Mathf.Clamp(targetPos.x,xMin,xMax);
        float yPos = targetPos.y + height + extraHeight;
        float zPos = Mathf.Clamp(targetPos.z - depth - extraDepth,zMin,zMax);
        targetPos = new Vector3(xPos,yPos,zPos);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
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
    public void ChangeRoom(BoxCollider box, float extraHeight, float extraDepth, float maxAngle)
    {
        this.box = box;
        this.extraHeight = extraHeight;
        this.maxAngle = maxAngle;
        this.extraDepth = extraDepth;
        xMax = box.bounds.center.x + box.bounds.extents.x;
        xMin = box.bounds.center.x - box.bounds.extents.x;
        zMax = box.bounds.center.z + box.bounds.extents.z;
        zMin = box.bounds.center.z - box.bounds.extents.z;
    }
    public void ChangeFocus(Transform target)
    {
        lookAt = target;
        this.target = target;
    }
    public float MaxBookHeight()
    {
        return box.bounds.center.y + box.bounds.extents.y;
    }

}





