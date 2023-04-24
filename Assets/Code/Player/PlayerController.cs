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
    float movementAcceleration;
    private Vector3 movement;
    private Vector3 direction;
    private Vector3 lastDirection;

    [Header("Jumping")]
    [SerializeField] private float coyoteTime;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
    private bool onGround = true;
    private Vector3 verticalSpeed;
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

    }

    private void SetUpJumpVariables()
    {

        maxJumpHeight += maxJumpHeight * 0.05f;
        float timeToApex = maxJumpTime / 2f; //The time to reach the maximum height of the jump.
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);

        jumpForce = (2 * maxJumpHeight) / timeToApex;
    }

    void Update()
    {

        //Movement

        Vector2 tempDirection = InputManager.GetAction("Move").context.ReadValue<Vector2>();
        Vector3 movDirection = new Vector3(tempDirection.x, 0, tempDirection.y);

        direction = movDirection;
        direction.Normalize();

        if (direction != Vector3.zero)
        {

            lastDirection = direction;
            if (movementAcceleration < 1f)
            {
                movementAcceleration += Mathf.Clamp01(tempDirection.magnitude) * increment * Time.deltaTime;
                movementAcceleration = Mathf.Clamp(movementAcceleration, 0, tempDirection.magnitude);
            }

        }
        else
        {

            movementAcceleration -= decrement * Time.deltaTime;

            movementAcceleration = Mathf.Clamp01(movementAcceleration);

        }

        rendererTransform.forward = Vector3.Dot(rendererTransform.forward, lastDirection) >= -.8f ?
            rendererTransform.forward = Vector3.Lerp(rendererTransform.forward, lastDirection, maxAngularSpeed * Time.deltaTime) : lastDirection;

        movement = maxLinealSpeed * lastDirection * Time.deltaTime * movementAcceleration;


        //Jump

        PushDetection();

        SetGravity();

        if (CanJump() && InputManager.GetAction("Jump").context.WasPerformedThisFrame())
        {
            /*float previousYVelocity = verticalSpeed.y;
            float newYVelocity = (verticalSpeed.y + jumpForce);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;

            verticalSpeed.y = nextYVelocity;*/

            verticalSpeed.y = jumpForce * .5f;
        }

        CollisionFlags l_collisionFlags = characterController.Move(movement);

        CheckCollision(l_collisionFlags);






        //Push
        if (currentObjectPushing != null)
        {
            Push();
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

        if(currentObjectPushing != null && isPushing && InputManager.GetAction("Test1").context.WasPressedThisFrame())
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

        currentObjectPushing.AddForceTowardsDirection(pushForce * movementAcceleration, direction);

    }

    private bool CanJump()
    {
        return onGround && !isPushing;
    }

    void SetGravity()
    {
        float previousYVelocity = verticalSpeed.y;
        float newYVelocity = verticalSpeed.y + (gravity * Time.deltaTime);
        float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;

        verticalSpeed.y = nextYVelocity;
        movement.y = verticalSpeed.y * Time.deltaTime;

    }

    void CheckCollision(CollisionFlags collisionFlag)
    {
        if ((collisionFlag & CollisionFlags.Above) != 0 && verticalSpeed.y > 0.0f)
        {
            verticalSpeed.y = 0.0f;
        }

        if ((collisionFlag & CollisionFlags.Below) != 0 && verticalSpeed.y < 0.0f)
        {

            verticalSpeed.y = 0.0f;
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

