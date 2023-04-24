using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using System.Reflection;

public class Book : MonoBehaviour
{
    GameObject bookGraphics;
    Transform objectHolder;
    Animator anim;
    public VisualEffect particles;
    GameObject target;
    public int angleIterations;
    public float distanceIterations;
    public float maxDistance; 
    public Transform player;
    public float groundDetectionDistance;
    public GameObject shapeTest;
    public LayerMask whatIsNotPlayer;
    Vector3 offset = new Vector3(0,0.05f,0);
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position,player.position+new Vector3(0,-groundDetectionDistance,0));
        Gizmos.DrawLine(player.position,player.position+new Vector3(maxDistance,0,0));
    }
    private void OnEnable() {
        InputManager.GetAction("Test1").action += Test;
    }
    private void OnDisable() {
        InputManager.GetAction("Test1").action -= Test;
    }
    void Test(InputAction.CallbackContext context)
    {
        if(context.started) Shapeshift(shapeTest);
    }

    public void Shapeshift(GameObject _target)
    {
        if(_target.Equals(null)) return;
        
        target = Instantiate(_target,objectHolder);
        target.AddComponent<ColliderSensor>();
        Collider[] cols = target.GetComponents<Collider>();
        foreach (Collider col in cols)
        {
            col.isTrigger = true;
        }

        if(!FindSpot()) return;

        target.GetComponent<ColliderSensor>().enabled = false;
        foreach (Collider col in cols)
        {
            col.isTrigger = false;
        }
    }
    public bool FindSpot()
    {
        ColliderSensor sensor = target.GetComponent<ColliderSensor>();
        Collider col = target.GetComponent<Collider>();
        for (float distI = 1; distI <= distanceIterations; distI++)
        {
            float distance = maxDistance/distanceIterations*distI;

            for (int angI = 1; angI <= angleIterations; angI++)
            {
                float angle = 360/angleIterations*angI;
                Vector3 desiredPosition =  player.position + OrientationToVector(player.eulerAngles.y + angle).normalized * distance + offset;
                target.transform.position = desiredPosition;

                //if(!VisibleToPlayer(col)) Debug.Log("Visible To Player FAILED");
                if(IsOverlapping(sensor)) Debug.Log("Overlapping FAILED");
                if(!OnGround()) Debug.Log("OnGround FAILED");

                //if(!VisibleToPlayer(col)) continue;
                if(IsOverlapping(sensor)) continue;
                if(!OnGround()) continue;
                return true;
            }
        }
        Debug.Log("Couldn't find a spot!");
        Destroy(target);
        return false;
    }
    bool VisibleToPlayer(Collider col)
    {
        RaycastHit hit;
        Physics.Raycast(player.position, col.bounds.center - player.position, out hit,Mathf.Infinity,whatIsNotPlayer,QueryTriggerInteraction.Collide);
        if(hit.collider!=null) return hit.collider.gameObject == target;
        else return false;
    }
    bool OnGround()
    {
        return Physics.Raycast(target.transform.position-offset/2,Vector3.down,groundDetectionDistance);
    }
    bool IsOverlapping(ColliderSensor sensor)
    {
        return sensor.IsOverlapping();
    }
    Vector3 OrientationToVector(float angle)
    {
        angle = angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos (angle);
        float sin = Mathf.Sin (angle);

        return new Vector3 (sin, 0, cos);
    }
    public static T CopyComponent<T> ( GameObject game, T duplicate ) where T : Component
    {
        T target = game.AddComponent<T> ();
        foreach (PropertyInfo x in typeof ( T ).GetProperties ())
            if (x.CanWrite)
                x.SetValue ( target, x.GetValue ( duplicate ) );
        return target;
    }
}
