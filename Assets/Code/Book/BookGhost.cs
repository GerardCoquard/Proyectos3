using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookGhost : MonoBehaviour
{
    public float speed;
    Vector2 movement;
    bool up;
    bool down;
    CharacterController characterController;
    private void Start() {
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable() {
        InputManager.GetAction("Move").action += OnMovementInput;
        InputManager.GetAction("Jump").action += OnUpInput;
        InputManager.GetAction("Shift").action += OnDownInput;
    }
    private void OnDisable() {
        InputManager.GetAction("Move").action -= OnMovementInput;
        InputManager.GetAction("Jump").action -= OnUpInput;
        InputManager.GetAction("Shift").action -= OnDownInput;
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
    private void Update() {
        int vertical = 0;
        if(up) vertical++;
        if(down) vertical--;
        Vector3 finalMovement = new Vector3(movement.x,vertical,movement.y).normalized;
        characterController.Move(finalMovement * Time.deltaTime * speed);
    }
}
