using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Transform finalPosition;
    [SerializeField] float timeToReach;
    Vector3 initPos;
    Vector3 finalPos;
    float distanceBetweenPositions;
    bool locked;

    private void Start()
    {
        initPos = transform.position;
        finalPos = finalPosition.position;
        distanceBetweenPositions = Vector3.Distance(initPos, finalPos);
    }
    public void Move()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine());
    }
    public void ResetMove()
    {
        if(locked) return;
        StopAllCoroutines();
        StartCoroutine(ResetObjectCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        float distanceToTarget = Vector3.Distance(transform.position, finalPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReach;
        Vector3 _initPos = transform.position;
        float timer = 0f;
        while (timer<time)
        {
            transform.position = Vector3.Lerp(_initPos, finalPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPos;
    }
    IEnumerator ResetObjectCoroutine()
    {
        float distanceToTarget = Vector3.Distance(transform.position, initPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReach;
        Vector3 _initPos = transform.position;
        float timer = 0f;
        while (timer<time)
        {
            transform.position = Vector3.Lerp(_initPos, initPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = initPos;
    }
    public void SetLocked(bool state)
    {
        locked = state;
    }
}
