using UnityEngine;

public class PusheableObject : MonoBehaviour
{
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddForceTowardsDirection(float force, Vector3 direction)
    {
        rb.velocity = direction * force * Time.fixedDeltaTime;
    }
}

