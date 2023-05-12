using System.Collections.Generic;
using UnityEngine;

public class LightBeam
{
    public Vector3 position;
    public Vector3 direction;
    public GameObject lightGameObject;
    public LineRenderer lineRenderer;
    List<Vector3> lightIndices = new List<Vector3>();
    public LayerMask layerMask;
    public Material material;
    List<LightReciever> lightRecieverList = new List<LightReciever>();
    List<LightReciever> currentLightRecivers = new List<LightReciever>();
    public int maxBounces;
    public RayColor rayType;

    public LightBeam(Vector3 pos, Vector3 dir, Material material, LayerMask layerMask, int maxBounces, RayColor _rayType)
    {
        lineRenderer = new LineRenderer();
        lightGameObject = new GameObject();
        lightGameObject.name = "LightBeam";
        position = pos;
        direction = dir;
        this.layerMask = layerMask;
        this.maxBounces = maxBounces;
        this.material = material;
        this.rayType = _rayType;

        lineRenderer = lightGameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = material;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        CastLight(position, direction, lineRenderer);
    }
    public LightBeam(LightBeam beam)
    {
        this.lineRenderer = new LineRenderer();
        this.lightGameObject = new GameObject();
        this.lightGameObject.name = "LightBeamExtra";
        this.position = beam.position;
        this.direction = beam.direction;
        this.layerMask = beam.layerMask;
        this.maxBounces = beam.maxBounces;
        this.material = beam.material;
        this.rayType = beam.rayType;

        lineRenderer = lightGameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = beam.material;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }
    public void ExecuteRay(Vector3 pos, Vector3 dir, LineRenderer renderer)
    {
        //Start the cast of the ray
        lineRenderer.positionCount = 0;
        lightIndices.Clear();
        CastLight(pos, dir, renderer);
        UpdateLightBeam();
        CheckRecievers();
    }
    public void CastLight(Vector3 pos, Vector3 dir, LineRenderer renderer)
    {
        lightIndices.Add(pos);

        Ray ray = new Ray(pos, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50, layerMask,QueryTriggerInteraction.Ignore) && lightIndices.Count < maxBounces)
        {
            CheckHit(hit, dir, renderer);
        }
        else
        {
            lightIndices.Add(ray.GetPoint(50));
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

    private void CheckRecievers()
    {
        List<LightReciever> tempList = new List<LightReciever>(lightRecieverList);

        foreach (LightReciever receiver in tempList)
        {
            if (!currentLightRecivers.Contains(receiver))
            {
                receiver.UndoAction(this);
                lightRecieverList.Remove(receiver);
            }
        }
        currentLightRecivers.Clear();
    }

    private void CheckHit(RaycastHit hitInfo, Vector3 direction, LineRenderer line)
    {
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.tag == "Mirror")
            {
                Vector3 pos = hitInfo.point;
                Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);
                CastLight(pos, dir, line);
            }
            else
            {
                lightIndices.Add(hitInfo.point);
            }
            if (hitInfo.collider.tag == "LightTrigger")
            {
                LightReciever reciver = hitInfo.collider.GetComponent<LightReciever>();
                if (!lightRecieverList.Contains(reciver))
                {
                    reciver.DoAction(this);
                    lightRecieverList.Add(reciver);
                }
                currentLightRecivers.Add(reciver);
                reciver.UpdatePoint(this,hitInfo.point,direction);
            }
        }
    }
}
public enum RayColor
{
    Red,
    Green,
    Blue,
    Purple,
    Yellow,
    Anyone
}
