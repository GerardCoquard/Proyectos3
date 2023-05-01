using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform origin;
    public LayerMask mask;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

    }

    private bool IsSomethingAbove(out RaycastHit collider)
    {
        Ray r = new Ray(origin.position, Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit, 1f, mask))
        {
            collider = hit;
            return true;
        }
        else
        {
            collider = hit;
            return false;
        }
    }
}

