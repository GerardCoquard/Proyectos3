using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMovement : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float seekWeight;
    public float wanderRate;
    Transform attractor;
    float wanderTargetOrientation;
    public float wanderRadius;
    public float wanderOffset;
    public float raycastDistance;
    public float sphereRadius;
    public float innerRadius;
    public float outRangeMultiplier;
    public float angleIterations;
    public LayerMask whatIsObstacles;
    public float fade;
    float currentSpeed;
    float currentAcceleration;
    float currentWeight;
    float inRangeAcceleration;
    Vector3 velocity;
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,sphereRadius);
        Gizmos.DrawWireSphere(transform.position,innerRadius);
        Gizmos.DrawLine(transform.position,transform.position+Vector3.forward*raycastDistance);
    }
    private void Start() {
        attractor = GameObject.FindGameObjectWithTag("BookAttractor").transform;
        inRangeAcceleration = acceleration;
    }
    void Update()
    {
        Move();
    }
    public void Move()
    {
        bool inRange = Vector3.Distance(transform.position,attractor.position) <= innerRadius;
        if(inRange)
        {
            currentSpeed-=fade*Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed,speed,speed*outRangeMultiplier);
            currentWeight = seekWeight;
            acceleration = inRangeAcceleration;
        }
        else
        {
            currentSpeed+=fade*Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed,speed,speed*outRangeMultiplier);
            currentWeight = 1;
            acceleration = 30;
        }
        Vector3 currentAcceleration = GetLinearAcceleration();
        Vector3 velIncrement = currentAcceleration * Time.deltaTime;
        velocity += velIncrement;
        velocity = Vector3.ClampMagnitude(velocity,currentSpeed);
        transform.position += velocity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x,attractor.position.y, transform.position.z);
    }
    public Vector3 GetLinearAcceleration()
    {
        if(Vector2.Distance(transform.position,attractor.position) >= innerRadius*3) return GetWanderAroundAcceleration();
        Vector3 avoidAcc = GetAvoidanceAcceleration();
        if(!avoidAcc.Equals(Vector3.zero)) Debug.Log("AVOIDING!");
        if (avoidAcc.Equals(Vector3.zero))
            return GetWanderAroundAcceleration();
        else
            return avoidAcc;
    }
    Vector3 GetAvoidanceAcceleration()
    {
        Vector3 hitPoint = Vector3.zero;
        for (int angI = 0; angI < angleIterations; angI++)
        {
            float angle = angI*360/angleIterations;
            if(Hit(OrientationToVector(angle),out RaycastHit hit))
            {
                if(hitPoint==Vector3.zero)
                {
                    hitPoint = hit.point;
                    continue;
                }
                if(Vector2.Distance(transform.position,hit.point) < Vector2.Distance(transform.position,hitPoint)) hitPoint = hit.point;
            }
        }
        if(hitPoint==Vector3.zero) return hitPoint;
        else
        {
            hitPoint = transform.position - hitPoint;
            return hitPoint.normalized*acceleration;
        }
    }
    bool Hit(Vector3 dir,out RaycastHit hit)
    {
        return Physics.Raycast(transform.position,dir,out hit,raycastDistance,whatIsObstacles, QueryTriggerInteraction.Ignore);
    }
    Vector3 GetWanderAroundAcceleration()
    {
        Vector3 seekAcc = attractor.position - transform.position;
        seekAcc = seekAcc.normalized * acceleration;
        Vector3 wanderAcc = GetWanderLinearAcceleration();

        return seekAcc*currentWeight + wanderAcc*(1-currentWeight);
    }
    Vector3 GetWanderLinearAcceleration()
    {
        wanderTargetOrientation += wanderRate * binomial();

        Vector3 surrogatePos = OrientationToVector(wanderTargetOrientation) * wanderRadius;

        if (velocity.magnitude>0.01f)
            surrogatePos +=
                transform.position + velocity.normalized * wanderOffset;
        else 
            surrogatePos += transform.position+ OrientationToVector(transform.eulerAngles.y) * wanderOffset;

        Vector3 finalDir = surrogatePos - transform.position;
        return finalDir.normalized * acceleration;
    }
    public static Vector3 OrientationToVector (float alpha) {
        alpha = alpha * Mathf.Deg2Rad;

        float cos = Mathf.Cos (alpha);
        float sin = Mathf.Sin (alpha);

        return new Vector3 (sin, 0, cos);
    }
    public static  float binomial () {
        return Random.value - Random.value;
    }
}
