using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using System.Reflection;

public class Book : MonoBehaviour
{
    public static Book instance;
    public Transform player;
    public float playerWidth;
    public int angleIterations;
    public int extraAngleIterationsOnDistance;
    public float distanceIterations = 1;
    public float iterationDistance;
    public float groundDetectionDistance;
    public GameObject bookGraphics;
    public GameObject bookGhost;
    public Transform particleStartPosition;
    public LayerMask whatIsNotPlayer;
    Vector3 offset = new Vector3(0,0.05f,0);
    public delegate void BookStateChanged(bool state);
    public static event BookStateChanged OnBookStateChanged;
    GameObject shapeshiftedObject;
    bool ghostActive;
    public Vector3 bookOffset;
    public VisualEffect particles;
    private void Awake() {
        if(instance==null) instance = this;
        else Destroy(this);
    }
    private void Update() {
        if(shapeshiftedObject==null)
        {
            transform.position = player.position + bookOffset;
        }
    }
    void ResetBook()
    {
        transform.position = player.position + bookOffset;
        if(shapeshiftedObject!=null)
        {
            Destroy(shapeshiftedObject.gameObject);
            shapeshiftedObject = null;
            particles.Play();

        }
        bookGraphics.SetActive(true);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position,player.position+new Vector3(0,-groundDetectionDistance,0));
        Gizmos.DrawLine(player.position,player.position+new Vector3(iterationDistance,0,0));
        Gizmos.DrawLine(player.position,player.position+new Vector3(-playerWidth,0,0));
    }
    private void OnEnable() {
        particles.Stop();
        bookGhost.transform.position = particleStartPosition.position;
        bookGhost.SetActive(false);
    }
    public void ActivateBook()
    {
        ResetBook();
        bookGhost.transform.position = particleStartPosition.position;
        bookGhost.SetActive(true);
        OnBookStateChanged?.Invoke(true);
        ghostActive = true;
    }
    public void DeactivateBook()
    {
        bookGhost.SetActive(false);
        OnBookStateChanged?.Invoke(false);
        ghostActive = false;
    }
    void SpotFound(Vector3 pos, GameObject clone)
    {
        shapeshiftedObject = Instantiate(clone,pos, clone.transform.rotation);
        Destroy(shapeshiftedObject.GetComponent<Shape>());
        particles.Play();
        bookGraphics.SetActive(false);
    }

    public void ShapeshiftBox(Shape shape, Vector3 extents)
    {
        float largerExtent = Mathf.Max(extents.x, extents.z);
        Vector3 extentVector = new Vector3(0,extents.y,0);
        for (int distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + largerExtent + iterationDistance*distI;
            
            for (int angI = 0; angI <= (angleIterations+(distI*extraAngleIterationsOnDistance))/2; angI++)
            {
                int extraAngles = distI*extraAngleIterationsOnDistance;
                int angle = 360/(angleIterations+extraAngles)*angI;
                bool inverted = false;
                for (int i = 0; i < 2; i++)
                {
                    if(angle == 180 || angle == 0) i++;
                    Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + (inverted? -angle : angle)).normalized * distance + offset;
                    inverted = true;
                    if(Physics.CheckBox(desiredPosition+extentVector,extents,Quaternion.identity,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                    if(!VisibleToPlayer(desiredPosition+extentVector)) continue;
                    if(!OnGround(desiredPosition,new Vector2(extents.x,extents.z))) continue;
                    SpotFound(desiredPosition,shape.gameObject);
                    return;
                }
                
            }
        }
    }
    public void ShapeshiftCapsule(Shape shape, float radius, float height)
    {
        Vector3 bottomPoint = new Vector3(0,radius,0);
        Vector3 topPoint = new Vector3(0,height - radius,0);
        for (int distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + radius + iterationDistance*distI;

            for (int angI = 0; angI <= (angleIterations+(distI*extraAngleIterationsOnDistance))/2; angI++)
            {
                int extraAngles = distI*extraAngleIterationsOnDistance;
                int angle = 360/(angleIterations+extraAngles)*angI;
                bool inverted = false;
                for (int i = 0; i < 2; i++)
                {
                    if(angle == 180 || angle == 0) i++;
                    Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + (inverted? -angle : angle)).normalized * distance + offset;
                    inverted = true;
                    if(Physics.CheckCapsule(desiredPosition+bottomPoint,desiredPosition+topPoint,radius,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                    if(!VisibleToPlayer(desiredPosition+new Vector3(0,height/2,0))) continue;
                    if(!OnGround(desiredPosition,new Vector2(radius,radius))) continue;
                    
                    SpotFound(desiredPosition,shape.gameObject);
                    return;
                }
            }
        }
    }
    public void ShapeshiftSphere(Shape shape, float radius)
    {
        Vector3 radiusVector = new Vector3(0,radius,0);
        for (int distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + radius + iterationDistance*distI;

            for (int angI = 0; angI <= (angleIterations+(distI*extraAngleIterationsOnDistance))/2; angI++)
            {
                int extraAngles = distI*extraAngleIterationsOnDistance;
                int angle = 360/(angleIterations+extraAngles)*angI;
                bool inverted = false;
                for (int i = 0; i < 2; i++)
                {
                    if(angle == 180 || angle == 0) i++;
                    Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + (inverted? -angle : angle)).normalized * distance + offset;
                    inverted = true;
                    if(Physics.CheckSphere(desiredPosition+radiusVector,radius,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                    if(!VisibleToPlayer(desiredPosition+radiusVector)) continue;
                    if(!OnGround(desiredPosition,new Vector2(radius,radius))) continue;
                    
                    SpotFound(desiredPosition,shape.gameObject);
                    return;
                }
            }
        }
    }
    bool VisibleToPlayer(Vector3 pos)
    {
        RaycastHit hit;
        return !Physics.Raycast(player.position, pos - player.position, out hit,Vector3.Distance(player.position,pos),whatIsNotPlayer,QueryTriggerInteraction.Ignore);
    }
    bool OnGround(Vector3 pos,Vector2 bounds)
    {
        bool corner1 = Physics.Raycast(pos+new Vector3(bounds.x,0,bounds.y),Vector3.down,groundDetectionDistance);
        bool corner2 = Physics.Raycast(pos+new Vector3(bounds.x,0,-bounds.y),Vector3.down,groundDetectionDistance);
        bool corner3 = Physics.Raycast(pos+new Vector3(-bounds.x,0,-bounds.y),Vector3.down,groundDetectionDistance);
        bool corner4 = Physics.Raycast(pos+new Vector3(-bounds.x,0,bounds.y),Vector3.down,groundDetectionDistance);
        return corner1 && corner2 && corner3 && corner4;
    }
    Vector3 OrientationToVector(float angle)
    {
        angle = angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos (angle);
        float sin = Mathf.Sin (angle);

        return new Vector3 (sin, 0, cos);
    }
}
