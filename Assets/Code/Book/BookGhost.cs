using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookGhost : MonoBehaviour
{
    public float speed;
    public float shapeDetectionRadius;
    public GameObject canvas;
    Vector2 movement;
    bool up;
    bool down;
    CharacterController characterController;
    Shape selectedShape;
    private void Start() {
        characterController = GetComponent<CharacterController>();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,0,-shapeDetectionRadius));
    }
    private void OnEnable() {
        InputManager.GetAction("Move").action += OnMovementInput;
        InputManager.GetAction("Jump").action += OnUpInput;
        InputManager.GetAction("Shift").action += OnDownInput;
        InputManager.GetAction("Push").action += OnInteractInput;
        canvas.SetActive(false);
        movement = Vector2.zero;
        up = false;
        down = false;
        if(InputManager.GetAction("Move").GetEnabled()) movement = InputManager.GetAction("Move").context.ReadValue<Vector2>();
        if(InputManager.GetAction("Jump").GetEnabled()) up = InputManager.GetAction("Jump").context.ReadValue<float>() == 1;
        if(InputManager.GetAction("Shift").GetEnabled()) down = InputManager.GetAction("Shift").context.ReadValue<float>() == 1;
    }
    private void OnDisable() {
        InputManager.GetAction("Move").action -= OnMovementInput;
        InputManager.GetAction("Jump").action -= OnUpInput;
        InputManager.GetAction("Shift").action -= OnDownInput;
        InputManager.GetAction("Push").action -= OnInteractInput;

        ClearSelected();
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
        Move();
    }
    bool Move()
    {
        int vertical = 0;
        if(up) vertical++;
        if(down) vertical--;
        Vector3 finalMovement = new Vector3(movement.x,vertical,movement.y).normalized;
        characterController.Move(finalMovement * Time.deltaTime * speed);
        return finalMovement != Vector3.zero;
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer != LayerMask.NameToLayer("Outline")) return;
        Shape newShape = other.GetComponent<Shape>();
        if(newShape== null) return;
        if(newShape==selectedShape) return;
        ClearSelected();
        selectedShape = newShape;
        selectedShape.SetSelected();
        canvas.transform.position = selectedShape.shapeCollider.bounds.center;
        canvas.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer != LayerMask.NameToLayer("Outline")) return;
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
        canvas.SetActive(false);
    }
    void SelectShape()
    {
        if(selectedShape==null) return;
        selectedShape.Shift();
        PlayerController.instance.SwapControl();
    }
}
