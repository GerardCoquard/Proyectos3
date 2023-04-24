using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Transform rendererTransform;
    CharacterController characterController;
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
    [SerializeField] float closeObjectDetection;
    [SerializeField] float pushForce;
    [SerializeField] float maxRayDistance;
    [SerializeField] LayerMask pusheableLayer;
    [SerializeField] Transform raycastInitialTransform;
    private PusheableObject currentObjectPushing;
    private bool isPushing;






    private void Awake()
    {

        characterController = GetComponent<CharacterController>();
        SetUpJumpVariables();
        InputManager.GetAction("Move").context.started += OnMovementInput;
        InputManager.GetAction("Move").context.canceled += OnMovementInput;
        InputManager.GetAction("Move").context.performed += OnMovementInput;

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
        //Push
        if (currentObjectPushing != null)
        {
            Push();
        }
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

        HandleRotation();
        PushDetection();

        CollisionFlags collisionFlags = characterController.Move(movement * Time.deltaTime);
        CheckCollision(collisionFlags);

        SetGravity();
        HandleJump();




    }
    private void HandleJump()
    {
        if (CanJump() && InputManager.GetAction("Jump").context.WasPerformedThisFrame())
        {
            movement.y = jumpForce * .5f;
        }

    }

    private void PushDetection()
    {
        Ray ray = new Ray(raycastInitialTransform.position, rendererTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, pusheableLayer))
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

        currentObjectPushing.AddForceTowardsDirection(pushForce, direction);

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

