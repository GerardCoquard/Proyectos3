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
    private bool onGround = true;
    private Vector3 verticalSpeed;
    private float timeOnAir;
    private float jumpForce;
    [SerializeField] private float coyoteTime;
    //[SerializeField] private float fallMultiplier;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float maxJumpTime;
     private float gravity;



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

        characterController.Move(movement);
        

    }


    private bool CanJump()
    {
        return onGround;
    }

    void SetGravity()
    {
        float previousYVelocity = verticalSpeed.y;
        float newYVelocity = verticalSpeed.y + (gravity * Time.deltaTime);
        float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;

        verticalSpeed.y = nextYVelocity;
        movement.y = verticalSpeed.y * Time.deltaTime;

        if (movement.y <= 0.0f && !onGround)
        {
            //Debug.Log(transform.position.y);
        }



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

