using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform player;
    public LineRenderer rope;
    public LayerMask collMask;
    public float maxWidth;
    public float minWidth;
    [Range(0f,0.1f)]
    public float tolerance;
    List<RopePoint> ropePositions = new List<RopePoint>();

    private void Awake() => AddPosToRope(rope.transform.position,Vector3.zero);
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if(ropePositions.Count <= 2) return;
        Vector3 lastPointToPlayer = player.position-ropePositions[ropePositions.Count - 2].point;
        Vector3 lastPoint = ropePositions[ropePositions.Count - 2].point + lastPointToPlayer.normalized*0.5f + lastPointToPlayer*0.3f;
        Gizmos.DrawLine(player.position,ropePositions[ropePositions.Count - 3].point);
        Gizmos.DrawLine(lastPoint,ropePositions[ropePositions.Count - 3].point);
    }
    private void Update()
    {
        UpdateRope();
    }
    void UpdateRope()
    {
        UpdateRopeGraphics();
        LastSegmentGoToPlayerPos();
        DetectCollisionEnter();
        if (ropePositions.Count > 2) DetectCollisionExits();
    }

    void DetectCollisionEnter()
    {
        RaycastHit hit;
        Vector3 pointPos = ropePositions[ropePositions.Count - 2].point;
        if (Physics.Linecast(player.position, pointPos, out hit,collMask))
        {
            Vector3 hitPoint = hit.point+hit.normal*tolerance;
            AddPosToRope(hitPoint,hit.normal);
        }
    }

    void DetectCollisionExits()
    {
        RaycastHit hit;
        Vector3 pointPos = ropePositions[ropePositions.Count - 3].point;
        Vector3 lastPointToPlayer = player.position-ropePositions[ropePositions.Count - 2].point;
        Vector3 lastPoint = ropePositions[ropePositions.Count - 2].point + lastPointToPlayer.normalized*0.5f + lastPointToPlayer*0.3f;
        if (!Physics.Linecast(player.position, pointPos, out hit,collMask))
        {
            if(!Physics.Linecast(lastPoint, pointPos, out hit,collMask)) ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    void AddPosToRope(Vector3 _pos,Vector3 _normal)
    {
        if(ropePositions.Count>0) ropePositions.RemoveAt(ropePositions.Count - 1);
        ropePositions.Add(new RopePoint(_pos,_normal));
        ropePositions.Add(new RopePoint(player.position,Vector3.zero));
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

    void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, player.position);
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
