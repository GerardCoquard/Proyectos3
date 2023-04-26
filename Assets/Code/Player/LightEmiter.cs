using UnityEngine;

public class LightEmiter : MonoBehaviour
{
    public LightBeam beam;
    public Material material;
    public LayerMask layerMask;

    private void Start()
    {
        beam = new LightBeam(transform.position, transform.forward, material, layerMask);
    }

    private void Update()
    {
        beam.lineRenderer.positionCount = 0;
        beam.lightIndices.Clear();
        beam.CastLight(transform.position, transform.right, beam.lineRenderer);
    }

}