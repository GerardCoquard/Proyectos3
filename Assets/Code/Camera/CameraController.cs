using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public static CameraController instance { get; private set; }
    BoxCollider box;
    Transform target;
    public RoomTrigger firstRoom;
    public float height;
    public float depth;
    public float lateralOffset;
    public float lateralSpeed;
    public float verticalOffset;
    public float verticalSpeed;
    float angle;
    float extraDepth;
    float extraHeight;
    float xMin;
    float xMax;
    float zMin;
    float zMax;
    Vector2 movement;
    Vector2 tempDirection;
    Vector2 movementAcceleration;
    Vector2 direction;

    private void Awake()
    {
        if (instance == null)
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
        transform.rotation = Quaternion.Euler(angle,0,0);
        ChangeFocus(PlayerController.instance.transform);
        firstRoom.ChangeRoom();
    }
    private void OnEnable() {
        InputManager.GetAction("Move").action += OnMovementInput;
        PlayerController.instance.OnBookActivated += () => ChangeFocus(Book.instance.bookGhost.transform);
        PlayerController.instance.OnPlayerActivated += () => ChangeFocus(PlayerController.instance.transform);
        PlayerController.instance.OnObjectPushed += () => ChangeFocus(PlayerController.instance.transform);//
        PlayerController.instance.OnStoppedPushing += () => ChangeFocus(PlayerController.instance.transform); //
    }
    private void OnDisable() {
        InputManager.GetAction("Move").action -= OnMovementInput;
        PlayerController.instance.OnBookActivated -= () => ChangeFocus(Book.instance.bookGhost.transform);
        PlayerController.instance.OnPlayerActivated -= () => ChangeFocus(PlayerController.instance.transform);
        PlayerController.instance.OnObjectPushed -= () => ChangeFocus(PlayerController.instance.transform);//
        PlayerController.instance.OnStoppedPushing -= () => ChangeFocus(PlayerController.instance.transform);//
    }
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        tempDirection = context.ReadValue<Vector2>();
        tempDirection.Normalize();
    }

    private void Update()
    {
        HandleDirection();
        HandlePosition();
        HandleRotation();
    }
    void HandleDirection()
    {
        if (tempDirection != Vector2.zero)
        {
            movementAcceleration += tempDirection * PlayerController.instance.acceleration * Time.deltaTime;
            movementAcceleration = Vector2.ClampMagnitude(movementAcceleration, tempDirection.magnitude);
        }
        else
        {
            movementAcceleration -= movementAcceleration * PlayerController.instance.acceleration * Time.deltaTime;
            movementAcceleration = Vector2.ClampMagnitude(movementAcceleration, 1);
        }
        
        movement = PlayerController.instance.maxLinealSpeed* movementAcceleration;
        direction = movement.normalized * movement.magnitude / PlayerController.instance.maxLinealSpeed;
    }
    private void HandlePosition()
    {
        Vector3 targetPos = target.position + new Vector3(direction.x,0,direction.y) * lateralOffset;
        float xPos = Mathf.Clamp(targetPos.x,xMin,xMax);
        float yPos = targetPos.y + height + extraHeight;
        float zPos = Mathf.Clamp(target.position.z - depth - extraDepth,zMin,zMax);
        targetPos = new Vector3(xPos,yPos,zPos);
        transform.position = new Vector3(Mathf.Lerp(transform.position.x,xPos,Time.deltaTime * lateralSpeed),Mathf.Lerp(transform.position.y,yPos,Time.deltaTime * lateralSpeed * 2),Mathf.Lerp(transform.position.z,zPos,Time.deltaTime * lateralSpeed * 2));
    }
    private void HandleRotation()
    {
        transform.rotation = Quaternion.Euler(Mathf.Lerp(transform.rotation.eulerAngles.x,angle-direction.y*verticalOffset,Time.deltaTime * verticalSpeed),0,0);
    }
    public void ChangeRoom(BoxCollider box, float extraHeight, float extraDepth)
    {
        this.box = box;
        this.extraHeight = extraHeight;
        this.extraDepth = extraDepth;
        angle = 90 - Vector2.Angle(new Vector2(depth+extraDepth,height+extraHeight).normalized, new Vector2(0,height+extraHeight).normalized);
        angle-=4;
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