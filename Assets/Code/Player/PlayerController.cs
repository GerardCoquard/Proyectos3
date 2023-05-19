using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("References")]
    [SerializeField] public CharacterController characterController;

    [Header("Movement")]
    [SerializeField] float maxLinealSpeed = 7f;
    [SerializeField] float acceleration;
    [SerializeField] float rotationFractionPerFrame = 45f;
    float currentSpeed;
    Vector3 movement;
    private Vector2 tempDirection;
    private Vector2 movementAcceleration;
   

    [Header("Jumping")]
    [SerializeField] private float maxJumpHeight = 4;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float gravityIncreseValue;
    float jumpForce;
    //[SerializeField] private float fallMultiplier;
    float gravity;
    float initialGravity;
    bool isJumping;

    [Header("Push")]
    [SerializeField] float closeEnoughtDetection = 0.5f;
    [SerializeField] float pushForce = 40f;
    [SerializeField] float angleDot = 0.8f;
    [SerializeField] Transform pushStartDetectionPoint;
    PusheableObject currentObjectPushing;
    bool bookOpened;

    [Header("Animation")]
    private Animator myAnimator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(this);
        SetUpJumpVariables();
        currentSpeed = maxLinealSpeed;
    }
    private void OnEnable()
    {
        InputManager.GetAction("Move").action += OnMovementInput;
        InputManager.GetAction("ChangeMode").action += OnChangeModeInput;
        InputManager.GetAction("Push").action += OnPushInput;
        InputManager.GetAction("Jump").action += OnJumpInput;
    }
    private void OnDisable()
    {
        InputManager.GetAction("Move").action -= OnMovementInput;
        InputManager.GetAction("ChangeMode").action -= OnChangeModeInput;
        InputManager.GetAction("Push").action -= OnPushInput;
        InputManager.GetAction("Jump").action -= OnJumpInput;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pushStartDetectionPoint.position, pushStartDetectionPoint.position + transform.forward * closeEnoughtDetection);
    }
    void OnPushInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentObjectPushing != null) StopPushing();
            else CheckPush();
        }
    }
    void OnChangeModeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SwapControl();
        }
    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started) Jump();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        tempDirection = context.ReadValue<Vector2>();
        tempDirection.Normalize();
    }
    public void SwapControl()
    {
        if (isJumping) return;
        if (bookOpened)
        {
            if (CanInteract()) return;
            Book.instance.DeactivateBook();
            InputManager.GetAction("Move").action += OnMovementInput;
            InputManager.GetAction("Push").action += OnPushInput;
            InputManager.GetAction("Jump").action += OnJumpInput;
            bookOpened = false;
            characterController.enabled = true;
            CameraController.instance.ChangeFocus(transform);
            if (InputManager.GetAction("Move").GetEnabled())
            {
                Vector2 tempDirection = InputManager.GetAction("Move").context.ReadValue<Vector2>();
                movement.x = tempDirection.x * currentSpeed;
                movement.z = tempDirection.y * currentSpeed;
                
            }
        }
        else
        {
            Book.instance.ActivateBook();
            InputManager.GetAction("Move").action -= OnMovementInput;
            InputManager.GetAction("Push").action -= OnPushInput;
            InputManager.GetAction("Jump").action -= OnJumpInput;
            bookOpened = true;
            movement = Vector3.zero;
            characterController.enabled = false;
            CameraController.instance.ChangeFocus(Book.instance.bookGhost.transform);
            StopPushing();
        }
    }
    private void SetUpJumpVariables()
    {
        maxJumpHeight += maxJumpHeight * 0.05f;
        float timeToApex = maxJumpTime / 2f; //The time to reach the maximum height of the jump.
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialGravity = gravity;
        jumpForce = (2 * maxJumpHeight) / timeToApex;
    }

    private void FixedUpdate()
    {
        if (currentObjectPushing != null && !isJumping) Push();
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = movement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = movement.z;
        Quaternion currentRotation = transform.rotation;

        if (tempDirection != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFractionPerFrame * Time.deltaTime);
        }
    }
    void Update()
    {
        if (characterController.enabled)
        {
            HandleRotation();
            HandleAcceleration();
            CollisionFlags collisionFlags = characterController.Move(movement * Time.deltaTime);
            CheckCollision(collisionFlags);
        }
        CheckPushAvailable();
        SetGravity();
    }

    private void HandleAcceleration()
    {
        if (tempDirection != Vector2.zero)
        {

            movementAcceleration += tempDirection * acceleration * Time.deltaTime;
            movementAcceleration = Vector2.ClampMagnitude(movementAcceleration, tempDirection.magnitude);
        }
        else
        {

            movementAcceleration -= movementAcceleration * acceleration * Time.deltaTime;
            movementAcceleration = Vector2.ClampMagnitude(movementAcceleration, 1);

        }
        Vector2 currentSpeedVector = maxLinealSpeed * movementAcceleration;
        movement.x = currentSpeedVector.x;
        movement.z = currentSpeedVector.y;
    }
    private void Jump()
    {
        if (CanJump())
        {
            movement.y = jumpForce * .5f;
            isJumping = true;
        }

    }
    void CheckPushAvailable()
    {
        PusheableObject pusheable;
        if (CanInteract() && PusheableDetected(out pusheable))
        {
            WorldScreenUI.instance.SetIcon(IconType.Push, pusheable.uiPosition.position);
        }
        else WorldScreenUI.instance.HideIcon(IconType.Push);
    }
    public bool CanInteract()
    {
        if (isJumping) return false;
        if (currentObjectPushing != null) return false;
        if (bookOpened) return false;
        if (!characterController.enabled) return false;
        return true;
    }
    bool PusheableDetected(out PusheableObject pusheable)
    {
        pusheable = null;

        if (isJumping) return false;

        Ray ray = new Ray(pushStartDetectionPoint.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, closeEnoughtDetection, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            pusheable = hit.collider.GetComponentInParent<PusheableObject>();
            if (pusheable != null && Vector3.Dot(transform.forward, -hit.normal) >= angleDot)
            {
                transform.forward = -hit.normal;
                return true;
            }
        }
        return false;
    }
    void CheckPush()
    {
        PusheableObject pusheable;
        if (PusheableDetected(out pusheable) && CanInteract())
        {
            currentObjectPushing = pusheable;
            currentObjectPushing.MakePusheable();
            characterController.enabled = false;
            transform.SetParent(currentObjectPushing.transform);
        }
    }
    void StopPushing()
    {
        if (currentObjectPushing == null) return;

        transform.SetParent(null);
        currentObjectPushing.NotPusheable();
        currentObjectPushing = null;
        characterController.enabled = true;
    }
    private void Push()
    {
        currentObjectPushing.AddForceTowardsDirection(pushForce, tempDirection);
    }

    private bool CanJump()
    {
        return currentObjectPushing == null && !isJumping;
    }

    void SetGravity()
    {

        if (isJumping)
        {
            gravity -= gravityIncreseValue * Time.deltaTime;
        }

        float previousYVelocity = movement.y;
        float newYVelocity = movement.y + (gravity * Time.deltaTime);
        float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;

        movement.y = nextYVelocity;

    }

    void CheckCollision(CollisionFlags collisionFlag)
    {
        if ((collisionFlag & CollisionFlags.Above) != 0 && movement.y > 0.0f)
        {
            movement.y = 0.0f;
        }

        if ((collisionFlag & CollisionFlags.Below) != 0 && movement.y < 0.0f)
        {
            movement.y = 0.0f;
            gravity = initialGravity;
            isJumping = false;
        }
    }
    public Vector2 GetDirection()
    {
        if(currentObjectPushing!=null) return tempDirection;
        else 
        {
            Vector2 dir = new Vector2(movement.x, movement.z);
            dir = dir.normalized*dir.magnitude/maxLinealSpeed;
            return dir.magnitude>0.01? dir : Vector2.zero;
        }
    }
}

