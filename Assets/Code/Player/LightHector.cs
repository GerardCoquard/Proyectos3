using UnityEngine;

public class Light : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float maxLightDistance;

    private float offset;
    public void StartLight(Vector3 direction)
    {
        Ray ray = new Ray(lineRenderer.transform.position + (transform.forward * offset), lineRenderer.transform.forward);
        float lightDistance = maxLightDistance;
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, lightDistance, layerMask))
        {
            lightDistance = Vector3.Distance(lineRenderer.transform.position, hit.point);

            if(hit.collider.tag == "REFRACTION_MIRROR")
            {
                hit.collider.GetComponent<LightReflectorObject>().CreateRefraction(lineRenderer.transform.forward, hit.normal);
            }

        }
        lineRenderer.SetPosition(0, new Vector3(0f, 0f, offset));
        lineRenderer.SetPosition(1, new Vector3(0f, 0f, lightDistance));
        lineRenderer.transform.forward = direction;

    }
}