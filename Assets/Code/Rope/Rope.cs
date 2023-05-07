using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform startPos;
    public Transform holder;
    public LineRenderer rope;
    public float maxWidth;
    public float minWidth;
    [Range(0f,0.1f)]
    public float tolerance = 0.01f;
    public GameObject colliderPrefab;
    List<RopePoint> ropePositions = new List<RopePoint>();
    bool onUse;
    LayerMask layerMask;
    List<BoxCollider> colliders = new List<BoxCollider>();

    private void Awake()
    {
       AddPosToRope(startPos.transform.position,Vector3.zero);
    }
    private void Start() {
        Transform[] childs = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in childs)
        {
            child.SetParent(null);
        }
        transform.position = Vector3.zero;
        layerMask = Physics.AllLayers;
        layerMask &= ~(1 << LayerMask.NameToLayer("Rope"));
        layerMask &= ~(1 << LayerMask.NameToLayer("Player"));
        UpdateRopeGraphics();
        UpdateLineWidth();
        AddLastCollider();
    }
    /* private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if(ropePositions.Count <= 2) return;
        Vector3 lastPointToPlayer = holder.position-ropePositions[ropePositions.Count - 2].point;
        Vector3 lastPoint = ropePositions[ropePositions.Count - 2].point + lastPointToPlayer.normalized*0.5f + lastPointToPlayer*0.3f;
        Gizmos.DrawLine(holder.position,ropePositions[ropePositions.Count - 3].point);
        Gizmos.DrawLine(lastPoint,ropePositions[ropePositions.Count - 3].point);
    } */
    private void Update()
    {
        if(onUse) UpdateRope();
    }
    void UpdateRope()
    {
        UpdateRopeGraphics();
        LastSegmentGoToPlayerPos();
        DetectCollisionEnter();
        if (ropePositions.Count > 2) DetectCollisionExits();
        UpdateLineWidth();
    }

    void DetectCollisionEnter()
    {
        RaycastHit hit;
        Vector3 pointPos = ropePositions[ropePositions.Count - 2].point;
        if (Physics.Linecast(holder.position, pointPos, out hit,layerMask,QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.gameObject == holder) return;
            Vector3 hitPoint = hit.point+hit.normal*tolerance;
            AddPosToRope(hitPoint,hit.normal);
        }
    }

    void DetectCollisionExits()
    {
        RaycastHit hit;
        Vector3 pointPos = ropePositions[ropePositions.Count - 3].point;
        Vector3 lastPointToPlayer = holder.position-ropePositions[ropePositions.Count - 2].point;
        Vector3 lastPoint = ropePositions[ropePositions.Count - 2].point + lastPointToPlayer.normalized*0.5f + lastPointToPlayer*0.3f;
        if (!Physics.Linecast(holder.position, pointPos, out hit,layerMask,QueryTriggerInteraction.Ignore))
        {
            if(!Physics.Linecast(lastPoint, pointPos, out hit,layerMask,QueryTriggerInteraction.Ignore))
            {
                ropePositions.RemoveAt(ropePositions.Count - 2);
                RemoveLastCollider();
            }
        }
    }

    void AddPosToRope(Vector3 _pos,Vector3 _normal)
    {
        if(ropePositions.Count>0) ropePositions.RemoveAt(ropePositions.Count - 1);
        ropePositions.Add(new RopePoint(_pos,_normal));
        ropePositions.Add(new RopePoint(holder.position,Vector3.zero));
        if(ropePositions.Count>2) AddCollider();
    }

    void UpdateRopeGraphics()
    {
        rope.positionCount = ropePositions.Count;

        for (int i = 0; i < rope.positionCount; i++)
        {
            Vector3 graphicPoint = ropePositions[i].point + ropePositions[i].normal * (rope.widthMultiplier/2-tolerance);
            rope.SetPosition(i,graphicPoint);
        }
    }
    void UpdateLineWidth()
    {

    }
    void AddCollider()
    {
        Vector3 pointA = ropePositions[ropePositions.Count-2].point;
        Vector3 pointB = ropePositions[ropePositions.Count-3].point;
        Vector3 pointAB = pointB-pointA;
        GameObject colGameObject = Instantiate(colliderPrefab,pointB-pointAB/2,Quaternion.identity,transform);
        colGameObject.transform.forward = Vector3.Cross(pointAB,Vector3.up);
        BoxCollider box = colGameObject.GetComponent<BoxCollider>();
        box.size = new Vector3(Vector3.Distance(pointA,pointB),rope.widthMultiplier,rope.widthMultiplier);
        colliders.Add(box);
    }
    void AddLastCollider()
    {
        Vector3 pointA = rope.GetPosition(rope.positionCount-1);
        Vector3 pointB = rope.GetPosition(rope.positionCount-2);
        Vector3 pointAB = pointB-pointA;
        GameObject colGameObject = Instantiate(colliderPrefab,pointB-pointAB/2,Quaternion.identity,transform);
        colGameObject.transform.forward = Vector3.Cross(pointAB,Vector3.up);
        BoxCollider box = colGameObject.GetComponent<BoxCollider>();
        box.size = new Vector3(Vector3.Distance(pointA,pointB),rope.widthMultiplier,rope.widthMultiplier);
        colliders.Add(box);
    }
    void RemoveLastCollider()
    {
        Destroy(colliders[colliders.Count-1].gameObject);
        colliders.RemoveAt(colliders.Count-1);
    }
    public void SetUse(bool state)
    {
        onUse = state;
        if(onUse) RemoveLastCollider();
        else AddLastCollider();
    }
    void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, holder.position);
    public struct RopePoint
    {
        public RopePoint(Vector3 _point,Vector3 _normal)
        {
            point = _point;
            normal = _normal;
        }
        public Vector3 point;
        public Vector3 normal;
    }
}
