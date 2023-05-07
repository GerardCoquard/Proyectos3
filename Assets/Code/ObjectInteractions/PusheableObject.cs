using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody))]
public class PusheableObject : MonoBehaviour
{
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
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
    public void AddForceTowardsDirection(float force, Vector3 direction)
    {
        rb.velocity = direction * force * Time.fixedDeltaTime;
    }
}

