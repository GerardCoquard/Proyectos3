using System.Collections.Generic;
using UnityEngine;

public class LightBeam
{
    private Vector3 position;
    private Vector3 direction;
    private GameObject lightGameObject;
    public LineRenderer lineRenderer;
    public List<Vector3> lightIndices = new List<Vector3>();

    private LightReciever lightReciever;
    private RaycastHit lastHitInfo;
    private LayerMask layerMask;


    public LightBeam(Vector3 pos, Vector3 dir, Material material, LayerMask layerMask)
    {
        lineRenderer = new LineRenderer();
        lightGameObject = new GameObject();
        lightGameObject.name = "LightBeam";
        position = pos;
        direction = dir;
        this.layerMask = layerMask;

        lineRenderer = lightGameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = material;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        CastLight(position, direction, lineRenderer);

    }
    public void CastLight(Vector3 pos, Vector3 dir, LineRenderer renderer)
    {
        lightIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, layerMask))
        {
            CheckHit(hit, dir, renderer);
        }
        else
        {
            lightIndices.Add(ray.GetPoint(30));
            UpdateLightBeam();
        }
    }

    private void UpdateLightBeam()
    {
        int count = 0;
        lineRenderer.positionCount = lightIndices.Count;

        foreach (Vector3 idx in lightIndices)
        {
            lineRenderer.SetPosition(count, idx);
            count++;
        }
    }

    private void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer line)
    {
        if (hitInfo.collider != null)
        {

            if (hitInfo.collider.tag == "Mirror")
            {
                if (hitInfo.collider != lastHitInfo.collider)
                {
                    lastHitInfo = hitInfo;
                }

                Vector3 pos = hitInfo.point;
                Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

                CastLight(pos, dir, line);
            }
            else
            {
                lightIndices.Add(hitInfo.point);
                UpdateLightBeam();
            }

            if (hitInfo.collider.tag == "LightTrigger")
            {

                if (hitInfo.collider != lastHitInfo.collider)
                {
                    lastHitInfo = hitInfo;
                }               

            }
            else if (lightReciever != null && lastHitInfo.collider.tag != "LightTrigger")
            {
                lightReciever.SetIsHitted(false);
                lightReciever = null;
            }
        }
        else if (lightReciever.GetIsHitted() == true && hitInfo.collider == null)
        {

            lightReciever.SetIsHitted(false);
            lightReciever = null;
        }

        if(lastHitInfo.collider.tag == "LightTrigger")
        {
            lightReciever = lastHitInfo.collider.GetComponent<LightReciever>();
            lightReciever.DoAction();
        }
    }


}
