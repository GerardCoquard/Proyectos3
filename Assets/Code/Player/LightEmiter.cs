using UnityEngine;

public class LightEmiter : MonoBehaviour
{
    LightBeam beam;
    public Transform rayStartPos;
    public Material material;
    public int maxBounces;

    private void Start()
    {
        beam = new LightBeam(transform.position, transform.forward, material, Physics.AllLayers,maxBounces);
    }

    private void Update()
    {
        beam.ExecuteRay(rayStartPos.position, rayStartPos.forward, beam.lineRenderer);
    }

}