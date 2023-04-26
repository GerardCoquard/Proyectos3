using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PusheableObject : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.useGravity = false;
        gameObject.layer = LayerMask.NameToLayer("Pusheable");
    }
    public void MakePusheable()
    {
        rb.isKinematic = false;
    }
    public void NotPusheable()
    {
        rb.isKinematic = true;
    }
    public void AddForceTowardsDirection(float force, Vector3 direction)
    {
        rb.velocity = direction * force * Time.fixedDeltaTime;
    }
}

