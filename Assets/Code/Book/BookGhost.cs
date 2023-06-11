using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookGhost : MonoBehaviour
{
    public float speed;
    public float shapeDetectionRadius;
    public Animator anim;
    Vector2 movement;
    bool up;
    bool down;
    CharacterController characterController;
    Shape selectedShape;
    bool delay;
    private void Start() {
        characterController = GetComponent<CharacterController>();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,0,-shapeDetectionRadius));
    }
    private void OnEnable() {
        StopAllCoroutines();
        StartCoroutine(Delay());
        InputManager.GetAction("Move").action += OnMovementInput;
        InputManager.GetAction("Jump").action += OnUpInput;
        InputManager.GetAction("Down").action += OnDownInput;
        InputManager.GetAction("Push").action += OnInteractInput;
        anim.SetTrigger("Appear");
        transform.localPosition = Vector3.zero;
        movement = Vector2.zero;
        up = false;
        down = false;
        if(InputManager.GetAction("Move").GetEnabled()) movement = InputManager.GetAction("Move").context.ReadValue<Vector2>();
        if(InputManager.GetAction("Jump").GetEnabled()) up = InputManager.GetAction("Jump").context.ReadValue<float>() == 1;
        if(InputManager.GetAction("Down").GetEnabled()) down = InputManager.GetAction("Down").context.ReadValue<float>() == 1;
    }
    private void OnDisable() {
        InputManager.GetAction("Move").action -= OnMovementInput;
        InputManager.GetAction("Jump").action -= OnUpInput;
        InputManager.GetAction("Down").action -= OnDownInput;
        InputManager.GetAction("Push").action -= OnInteractInput;
        transform.localPosition = Vector3.zero;
        ClearSelected();

        WorldScreenUI.instance.HideIcon(IconType.Book);
    }
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    private void OnUpInput(InputAction.CallbackContext context)
    {
        up = context.ReadValueAsButton();
    }
    private void OnDownInput(InputAction.CallbackContext context)
    {
        down = context.ReadValueAsButton();
    }
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.started) SelectShape();
    }
    private void Update() {
        if(!delay) return;
        Move();
        if(selectedShape != null) WorldScreenUI.instance.SetIcon(IconType.Book, transform.position+new Vector3(0,0.6f,0));
        else WorldScreenUI.instance.HideIcon(IconType.Book);
    }
    void Move()
    {
        int vertical = 0;
        if(up && transform.position.y<CameraController.instance.MaxBookHeight()) vertical++;
        if(down) vertical--;
        Vector3 finalMovement = new Vector3(movement.x,vertical,movement.y).normalized;
        
        characterController.Move(finalMovement * Time.deltaTime * speed);
    }
    public static Quaternion TiltRotationTowardsVelocity(Quaternion cleanRotation, Vector3 referenceUp, Vector3 vel, float velMagFor45Degree)
    {
        Vector3 rotAxis = Vector3.Cross( referenceUp, vel );
        float tiltAngle = Mathf.Atan( vel.magnitude /velMagFor45Degree) *Mathf.Rad2Deg;
        return Quaternion.AngleAxis( tiltAngle, rotAxis ) * cleanRotation;
    }
    private void OnTriggerEnter(Collider other) {
        Shape newShape = other.GetComponent<Shape>();
        if(newShape== null) return;
        if(newShape==selectedShape) return;
        ClearSelected();
        selectedShape = newShape;
        selectedShape.SetSelected();
    }
    private void OnTriggerExit(Collider other) {
        Shape newShape = other.GetComponent<Shape>();
        if(newShape== null) return;
        if(newShape!=selectedShape) return;
        ClearSelected();
    }
    void ClearSelected()
    {
        if(selectedShape==null) return;
        selectedShape.Unselect();
        selectedShape = null;
    }
    void SelectShape()
    {
        if(selectedShape==null) return;
        Book.instance.Shapehift(selectedShape,selectedShape.shapeCollider.bounds.extents);
    }
    IEnumerator Delay()
    {
        delay = false;
        yield return new WaitForSeconds(0.25f);
        delay = true;
    }
}
