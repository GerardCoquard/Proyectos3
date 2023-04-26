using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform cam;


    [Header("Movement")]
    [SerializeField] float maxLinealSpeed;
    [SerializeField] float maxAngularSpeed;
    [SerializeField] float increment;
    [SerializeField] float decrement;
    [SerializeField] float rotationFractionPerFrame;
    private bool isMovementPressed;
    private Vector3 movement;
    private Vector3 direction;

    [Header("Jumping")]
    [SerializeField] private float coyoteTime;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float groundedGravity;
    private bool onGround = true;
    private float timeOnAir;
    private float jumpForce;
    //[SerializeField] private float fallMultiplier;
    private float gravity;

    [Header("Push")]
    [SerializeField] float closeEnoughtDetection;
    [SerializeField] float pushForce;
    [SerializeField] Transform pushStartDetectionPoint;
    private PusheableObject currentObjectPushing;
    private bool isPushing;
    bool bookOpened;
    private void Awake()
    {
        if(instance==null) instance = this;
        else Destroy(this);
        SetUpJumpVariables();
    }
    private void OnEnable() {
        InputManager.GetAction("Move").action += OnMovementInput;
        //InputManager.GetAction("ChangeMode").action += OnChangeModeInput;
        InputManager.GetAction("Push").action += OnPushInput;
    }
    private void OnDisable() {
        InputManager.GetAction("Move").action -= OnMovementInput;
        //InputManager.GetAction("ChangeMode").action += OnChangeModeInput;
        InputManager.GetAction("Push").action -= OnPushInput;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pushStartDetectionPoint.position,pushStartDetectionPoint.position+transform.forward*closeEnoughtDetection);
    }
    void OnPushInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(currentObjectPushing!=null) StopPushing();
            else CheckPush();
        }
    }
    void OnChangeModeInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(bookOpened)
            {
                Book.instance.DeactivateBook();
                InputManager.GetAction("Move").action -= OnMovementInput;
            }
            else
            {
                Book.instance.ActivateBook();
                InputManager.GetAction("Move").action += OnMovementInput;
                movement = new Vector3(0,movement.y,0);
                StopPushing();
            }
        }
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        Vector2 tempDirection = context.ReadValue<Vector2>();
        movement.x = tempDirection.x * maxLinealSpeed;
        movement.z = tempDirection.y * maxLinealSpeed;
        isMovementPressed = tempDirection.x != 0 || tempDirection.y != 0;
    }
    private void SetUpJumpVariables()
    {
        maxJumpHeight += maxJumpHeight * 0.05f;
        float timeToApex = maxJumpTime / 2f; //The time to reach the maximum height of the jump.
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);

        jumpForce = (2 * maxJumpHeight) / timeToApex;
    }

    private void FixedUpdate()
    {
        if (currentObjectPushing != null) Push();
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = movement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = movement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFractionPerFrame * Time.deltaTime);
        }
    }
    void Update()
    {
        if(characterController.enabled)
        {
            HandleRotation();
            CollisionFlags collisionFlags = characterController.Move(movement * Time.deltaTime);
            CheckCollision(collisionFlags);
        }
        SetGravity();
        HandleJump();
    }
    private void HandleJump()
    {
        if (CanJump() && InputManager.GetAction("Jump").context.WasPerformedThisFrame()) movement.y = jumpForce * .5f;
    }

    private void PushDetection()
    {
        Ray ray = new Ray(transform.position + characterController.center, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, closeEnoughtDetection, LayerMask.GetMask("Pusheable")))
        {
            if (hit.collider != null)
            {
                if (InputManager.GetAction("Test1").context.WasPressedThisFrame() && !isPushing)
                {
                    //My player is pushing
                    currentObjectPushing = hit.collider.GetComponent<PusheableObject>();
                    currentObjectPushing.rb.isKinematic = false;
                    characterController.enabled = false;
                    transform.SetParent(currentObjectPushing.transform);
                    SetIsPushing(true);

                }
            }
        }

        if (currentObjectPushing != null && isPushing && InputManager.GetAction("Test1").context.WasPressedThisFrame())
        {
            currentObjectPushing.rb.isKinematic = true;
            currentObjectPushing = null;
            SetIsPushing(false);
            characterController.enabled = true;
            transform.SetParent(null);
        }
    }
    bool PusheableDetected(out PusheableObject pusheable)
    {
        pusheable = null;

        if(!onGround) return false;

        Ray ray = new Ray(pushStartDetectionPoint.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, closeEnoughtDetection, LayerMask.GetMask("Pusheable"),QueryTriggerInteraction.Ignore))
        {
            pusheable = hit.collider.GetComponent<PusheableObject>();
            return true;
        }
        return false;
    }
    void CheckPush()
    {
        PusheableObject pusheable;
        if (PusheableDetected(out pusheable))
        {
            currentObjectPushing = pusheable;
            currentObjectPushing.MakePusheable();
            characterController.enabled = false;
            transform.SetParent(currentObjectPushing.transform);
            SetIsPushing(true);
        }
    }
    void StopPushing()
    {
        if(currentObjectPushing == null) return;

        currentObjectPushing.NotPusheable();
        currentObjectPushing = null;
        characterController.enabled = true;
        transform.SetParent(null);
        SetIsPushing(false);
    }

    private void SetIsPushing(bool state)
    {
        StartCoroutine(VariableDelay(state));
    }

    IEnumerator VariableDelay(bool state)
    {
        yield return new WaitForEndOfFrame();
        isPushing = state;
    }

    private void Push()
    {
        currentObjectPushing.AddForceTowardsDirection(pushForce, movement);
    }

    private bool CanJump()
    {
        return onGround && !isPushing;
    }

    void SetGravity()
    {
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
            timeOnAir = 0.0f;
            onGround = true;
        }
        else
        {
            timeOnAir += Time.deltaTime;
            if (timeOnAir > coyoteTime)
            {
                onGround = false;
            }
        }
    }
}

