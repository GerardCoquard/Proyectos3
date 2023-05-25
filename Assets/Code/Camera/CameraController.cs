using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }
    BoxCollider box;
    Transform target;
    public RoomTrigger firstRoom;
    public float angle;
    public float height;
    public float depth;
    public float lateralOffset;
    public float lateralSpeed;
    public float verticalOffset;
    public float verticalSpeed;
    float extraDepth;
    float extraHeight;
    float xMin;
    float xMax;
    float zMin;
    float zMax;

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
        transform.rotation = Quaternion.Euler(angle,0,0);
        ChangeFocus(PlayerController.instance.transform);
        firstRoom.ChangeRoom();
    }

    private void Update()
    {
        HandlePosition();
        HandleRotation();
    }
    private void HandlePosition()
    {
        Vector3 targetPos = target.position + new Vector3(PlayerController.instance.GetDirection().x,0,PlayerController.instance.GetDirection().y) * lateralOffset;
        float xPos = Mathf.Clamp(targetPos.x,xMin,xMax);
        float yPos = targetPos.y + height + extraHeight;
        float zPos = Mathf.Clamp(target.position.z - depth - extraDepth,zMin,zMax);
        targetPos = new Vector3(xPos,yPos,zPos);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x,xPos,Time.deltaTime * lateralSpeed),Mathf.Lerp(transform.position.y,yPos,Time.deltaTime * lateralSpeed * 2),zPos);
    }
    private void HandleRotation()
    {
        transform.rotation = Quaternion.Euler(Mathf.Lerp(transform.rotation.eulerAngles.x,angle-PlayerController.instance.GetDirection().y*verticalOffset,Time.deltaTime * verticalSpeed),0,0);
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
        this.extraDepth = extraDepth;
        xMax = box.bounds.center.x + box.bounds.extents.x;
        xMin = box.bounds.center.x - box.bounds.extents.x;
        zMax = box.bounds.center.z + box.bounds.extents.z;
        zMin = box.bounds.center.z - box.bounds.extents.z;
    }
    public void ChangeFocus(Transform target)
    {
        this.target = target;
    }
    public float MaxBookHeight()
    {
        return box.bounds.center.y + box.bounds.extents.y;
    }

}





