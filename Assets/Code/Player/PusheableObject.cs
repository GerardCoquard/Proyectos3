using UnityEngine;

public class PusheableObject : MonoBehaviour
{
    public Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

