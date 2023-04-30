using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Rail rail;
    public Transform lookAt;
    public float moveSpeed = 5.0f;

    private Transform myTransform;
    private PlayerController controller;
    private Vector3 lastPosition;

    private float initialFOV;
    private Camera myCamera;
    public float distanceOfReference = 260f;
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        initialFOV = myCamera.fieldOfView;
        controller = FindObjectOfType<PlayerController>();
        myTransform = transform;
        lastPosition = myTransform.position;
    }

    private void Update()
    {
        lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), Time.deltaTime * moveSpeed);
        myTransform.position = lastPosition;
        myTransform.LookAt(lookAt.position);

        float distanceToTarget = (lookAt.position - myTransform.position).sqrMagnitude;
        myCamera.fieldOfView = initialFOV - (initialFOV/distanceToTarget) * initialFOV;
    }


}
