using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMovement : MonoBehaviour
{
    public float maxSpeed;
    public float minSpeed;
    public float maxAcceleration;
    public float minAcceleration;
    public float seekWeight;
    public float wanderRate;
    Transform attractor;
    float wanderTargetOrientation;
    public float wanderRadius;
    public float wanderOffset;
    public float raycastDistance;
    public float innerRadius;
    public float angleIterations;
    public LayerMask whatIsObstacles;
    public float fadeSpeedVelocity;
    public float fadeAccelerationVelocity;
    public float lateralOffset;
    float currentSpeed;
    float _currentAcceleration;
    float currentWeight;
    Vector3 velocity;
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,innerRadius);
        Gizmos.DrawLine(transform.position,transform.position+Vector3.forward*raycastDistance);
    }
    private void Start() {
        attractor = GameObject.FindGameObjectWithTag("BookAttractor").transform;
        transform.position = attractor.position + new Vector3(lateralOffset,0,0);
        currentSpeed = minSpeed;
        _currentAcceleration = minAcceleration;
        currentWeight = seekWeight;
    }
    void Update()
    {
        Move();
    }
    public void Move()
    {
        bool inRange = Vector3.Distance(transform.position,attractor.position+new Vector3(lateralOffset,0,0)) <= innerRadius;
        if(inRange)
        {
            currentSpeed-=fadeSpeedVelocity*Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed,minSpeed,maxSpeed);
            currentWeight = seekWeight;
            _currentAcceleration-=fadeAccelerationVelocity*Time.deltaTime;
            _currentAcceleration = Mathf.Clamp(_currentAcceleration,minAcceleration,maxAcceleration);
        }
        else
        {
            currentSpeed+=fadeSpeedVelocity*Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed,minSpeed,maxSpeed);
            currentWeight = 1;
            _currentAcceleration+=fadeAccelerationVelocity*Time.deltaTime;
            _currentAcceleration = Mathf.Clamp(_currentAcceleration,minAcceleration,maxAcceleration);
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
        if(Vector2.Distance(transform.position,attractor.position+new Vector3(lateralOffset,0,0)) >= innerRadius*3) return GetWanderAroundAcceleration();
        Vector3 avoidAcc = GetAvoidanceAcceleration();
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
            return hitPoint.normalized*_currentAcceleration;
        }
    }
    bool Hit(Vector3 dir,out RaycastHit hit)
    {
        return Physics.Raycast(transform.position,dir,out hit,raycastDistance,whatIsObstacles, QueryTriggerInteraction.Ignore);
    }
    Vector3 GetWanderAroundAcceleration()
    {
        Vector3 seekAcc = attractor.position+new Vector3(lateralOffset,0,0) - transform.position;
        seekAcc = seekAcc.normalized * _currentAcceleration;
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
        return finalDir.normalized * _currentAcceleration;
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
