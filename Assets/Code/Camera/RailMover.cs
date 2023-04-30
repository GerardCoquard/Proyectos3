using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Rail rail;
    public Transform lookAt;
    public float moveSpeed = 5.0f;
    public float interactFOV = 52f;
    public float zoomSpeed = 5f;
    private float initialFOV;

    private Transform myTransform;
    private Vector3 lastPosition;
    private Quaternion initialRotation;

    private Camera myCamera;
    private PlayerController controller;
    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        myCamera = GetComponent<Camera>();
        initialFOV = myCamera.fieldOfView;
        initialRotation = myCamera.transform.rotation;
        myTransform = transform;
        lastPosition = myTransform.position;
    }

    private void Update()
    {
        lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
        myTransform.position = lastPosition;

        HandlePlayerOnCamera();
        HandleZoomOnPlayer();
    }

    private void HandlePlayerOnCamera()
    {
        //Get player position on screen coordinates
        Vector3 playerPosToScreen = GetPlayerPosOnScreen(lookAt.position);
        //Check if player is out of boundries

        //Set the camera to its new rotation
    }

    private void HandleZoomOnPlayer()
    {
        //if controller is interacting zoom in
        //if not zoom out
        if (controller.isInteracting)
        {
            float delta = (interactFOV - myCamera.fieldOfView) * Time.deltaTime * zoomSpeed;
            myCamera.fieldOfView += delta;

            Quaternion targetRotation = Quaternion.LookRotation(lookAt.transform.position - transform.position);
            myCamera.transform.rotation = Quaternion.Slerp(myCamera.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);

        }
        else
        {
            float delta = (myCamera.fieldOfView - initialFOV) * Time.deltaTime * zoomSpeed;
            myCamera.fieldOfView -= delta;

            myCamera.transform.rotation = Quaternion.Slerp(myCamera.transform.rotation, initialRotation, moveSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetPlayerPosOnScreen(Vector3 pos)
    {
        return myCamera.WorldToScreenPoint(pos);
    }

}

