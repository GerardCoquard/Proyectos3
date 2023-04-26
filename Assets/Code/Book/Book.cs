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
    public float distanceIterations = 1;
    public float iterationDistance;
    public float groundDetectionDistance;
    public GameObject bookParticle;
    public Transform particleStartPosition;
    public CharacterController characterController;
    public Shape shapeTest;
    Vector3 offset = new Vector3(0,0.05f,0);
    private void Awake() {
        if(instance==null) instance = this;
        else Destroy(this);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position,player.position+new Vector3(0,-groundDetectionDistance,0));
        Gizmos.DrawLine(player.position,player.position+new Vector3(iterationDistance,0,0));
        Gizmos.DrawLine(player.position,player.position+new Vector3(-playerWidth,0,0));
    }
    private void OnEnable() {
        InputManager.GetAction("Test1").action += Test;
        bookParticle.transform.position = particleStartPosition.position;
        bookParticle.SetActive(false);
    }
    private void OnDisable() {
        InputManager.GetAction("Test1").action -= Test;
    }
    void Test(InputAction.CallbackContext context)
    {
        if(context.started) shapeTest.Shift();
    }
    public void ActivateBook()
    {
        bookParticle.transform.position = particleStartPosition.position;
        bookParticle.SetActive(true);
    }
    public void DeactivateBook()
    {
        bookParticle.transform.position = particleStartPosition.position;
        bookParticle.SetActive(false);
    }
    void SpotFound(Vector3 pos, GameObject clone)
    {
        GameObject target = Instantiate(clone,pos, clone.transform.rotation);
        Destroy(target.GetComponent<Shape>());
    }

    public void ShapeshiftBox(Shape shape, Vector3 extents)
    {
        float largerExtent = Mathf.Max(extents.x, extents.z);
        Vector3 extentVector = new Vector3(0,extents.y,0);
        for (float distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + largerExtent + iterationDistance*distI;

            for (int angI = 1; angI <= angleIterations; angI++)
            {
                float angle = 360/angleIterations*angI;
                Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + angle).normalized * distance + offset;

                if(Physics.CheckBox(desiredPosition+extentVector,extents,Quaternion.identity,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                if(!VisibleToPlayer(desiredPosition+extentVector)) continue;
                if(!OnGround(desiredPosition,new Vector2(extents.x,extents.z))) continue;
                
                SpotFound(desiredPosition,shape.gameObject);
                return;
            }
        }
    }
    public void ShapeshiftCapsule(Shape shape, float radius, float height)
    {
        Vector3 bottomPoint = new Vector3(0,radius,0);
        Vector3 topPoint = new Vector3(0,height - radius,0);
        for (float distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + radius + iterationDistance*distI;

            for (int angI = 1; angI <= angleIterations; angI++)
            {
                float angle = 360/angleIterations*angI;
                Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + angle).normalized * distance + offset;

                if(Physics.CheckCapsule(desiredPosition+bottomPoint,desiredPosition+topPoint,radius,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                if(!VisibleToPlayer(desiredPosition+new Vector3(0,height/2,0))) continue;
                if(!OnGround(desiredPosition,new Vector2(radius,radius))) continue;
                
                SpotFound(desiredPosition,shape.gameObject);
                return;
            }
        }
    }
    public void ShapeshiftSphere(Shape shape, float radius)
    {
        Vector3 radiusVector = new Vector3(0,radius,0);
        for (float distI = 0; distI < distanceIterations; distI++)
        {
            float distance = playerWidth + radius + iterationDistance*distI;

            for (int angI = 1; angI <= angleIterations; angI++)
            {
                float angle = 360/angleIterations*angI;
                Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + angle).normalized * distance + offset;

                if(Physics.CheckSphere(desiredPosition+radiusVector,radius,Physics.AllLayers,QueryTriggerInteraction.Ignore)) continue;
                if(!VisibleToPlayer(desiredPosition+radiusVector)) continue;
                if(!OnGround(desiredPosition,new Vector2(radius,radius))) continue;
                
                SpotFound(desiredPosition,shape.gameObject);
                return;
            }
        }
    }
    bool VisibleToPlayer(Vector3 pos)
    {
        RaycastHit hit;
        return !Physics.Raycast(player.position, pos - player.position, out hit,Vector3.Distance(player.position,pos),Physics.AllLayers & ~(1 << LayerMask.GetMask("Player")),QueryTriggerInteraction.Ignore);
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
