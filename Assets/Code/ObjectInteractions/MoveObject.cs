using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Transform platform;
    [SerializeField] Transform initialPosition;
    [SerializeField] Transform finalPosition;
    public float timeToReachDestination;
    Vector3 initPos;
    Vector3 finalPos;
    float distanceBetweenPositions;

    private void Start()
    {
        initPos = initialPosition.position;
        finalPos = finalPosition.position;
        platform.position = initPos;
        distanceBetweenPositions = Vector3.Distance(initPos, finalPos);
    }
    public void MovePlatform()
    {
        StopAllCoroutines();
        StartCoroutine(MovePlatformCoroutine());
    }

    public void ResetPlatform()
    {
        StopAllCoroutines();
        StartCoroutine(ResetPlatformCoroutine());
    }

    IEnumerator MovePlatformCoroutine()
    {
        float distanceToTarget = Vector3.Distance(platform.position, finalPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReachDestination;
        Vector3 _initPos = platform.position;
        float timer = 0f;
        while (timer<time)
        {
            platform.position = Vector3.Lerp(_initPos, finalPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        platform.position = finalPos;
    }
    IEnumerator ResetPlatformCoroutine()
    {
        float distanceToTarget = Vector3.Distance(platform.position, initPos);
        float time = distanceToTarget / distanceBetweenPositions * timeToReachDestination;
        Vector3 _initPos = platform.position;
        float timer = 0f;
        while (timer<time)
        {
            platform.position = Vector3.Lerp(_initPos, initPos, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        platform.position = initPos;

    }





}
