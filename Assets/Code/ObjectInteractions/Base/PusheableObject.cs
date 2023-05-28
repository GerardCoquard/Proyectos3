using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
[RequireComponent(typeof(Rigidbody))]
public class PusheableObject : MonoBehaviour
{
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    [NonSerialized]
    public Rigidbody rb;
    bool constrained;
    Vector3 constrainDirection;
    public Transform uiPosition;
    public float weightMultiplier = 1f;
    public GameObject particles;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
    private void OnEnable() {
        PlayerController.instance.OnObjectPushed += HideParticles;
        PlayerController.instance.OnStoppedPushing += ShowParticles;
        PlayerController.instance.OnBookActivated += HideParticles;
        PlayerController.instance.OnPlayerActivated += ShowParticles;
    }
    private void OnDisable() {
        PlayerController.instance.OnObjectPushed -= HideParticles;
        PlayerController.instance.OnStoppedPushing -= ShowParticles;
        PlayerController.instance.OnBookActivated -= HideParticles;
        PlayerController.instance.OnPlayerActivated -= ShowParticles;
    }
    public void MakePusheable()
    {
        rb.isKinematic = false;
        OnSelected?.Invoke();
    }
    public void NotPusheable()
    {
        rb.isKinematic = true;
        OnUnselected?.Invoke();
    }
    public void AddForceTowardsDirection(float force, Vector2 direction)
    {
        direction.Normalize();
        Vector3 newDir = new Vector3(direction.x,0,direction.y);
        if(constrained)
        {
            if(Vector3.Dot(newDir,constrainDirection) <= 0)
            {
                rb.velocity = Vector3.zero;
                return;
            }
        }

        rb.velocity = newDir * force * weightMultiplier * Time.fixedDeltaTime;
    }
    public void SetConstraint(bool state, Vector3 ropeDirection)
    {
        constrained = state;
        constrainDirection = new Vector3(ropeDirection.x,0,ropeDirection.z).normalized;
    }
    void ShowParticles()
    {
        particles.GetComponent<ParticleSystem>().Play();
    }
    void HideParticles()
    {
        particles.GetComponent<ParticleSystem>().Stop();
    }
}

