using UnityEngine;

public class LightEmiter : MonoBehaviour
{
    LightBeam beam;
    public Transform rayStartPos;
    public float width;
    public Material material;
    public int maxBounces;
    public RayColor rayColor = RayColor.Anyone;

    private void Start()
    {
        beam = new LightBeam(transform.position, transform.forward, material, Physics.AllLayers,maxBounces,rayColor,width);
    }

    private void Update()
    {
        beam.ExecuteRay(rayStartPos.position, rayStartPos.forward, beam.lineRenderer);
    }

}