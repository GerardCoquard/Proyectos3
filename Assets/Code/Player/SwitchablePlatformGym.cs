using System.Collections;
using UnityEngine;

public class SwitchablePlatformGym : MonoBehaviour
{
    [SerializeField] private Transform finalPlatformPosition;
    [SerializeField] Transform startPosition;
    [SerializeField] private float speed;
    private float timeToReachDestination;
    private float distanceBetweenPositions;
    bool isRestarting;

    private void Start()
    {
        distanceBetweenPositions = Vector3.Distance(finalPlatformPosition.position, startPosition.position);
    }
    public void SetPlatformToNewPosition()
    {

        StopAllCoroutines();
        StartCoroutine(MovePlatform());

    }

    public void ResetPlatformPosition()
    {

        StopAllCoroutines();
        StartCoroutine(ResetPlatform());
    }

    IEnumerator MovePlatform()
    {
        float distanceToTarget = Vector3.Distance(finalPlatformPosition.position, transform.position);
        float time = distanceToTarget / distanceBetweenPositions * timeToReachDestination;
        Vector3 initPos = transform.position;
        float timer = 0f;
        while (Mathf.Abs(distanceToTarget) > 0.1f)
        {
            distanceToTarget = Vector3.Distance(finalPlatformPosition.position, transform.position);
            transform.position = Vector3.Lerp(initPos, finalPlatformPosition.position, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        //transform.position = finalPlatformPosition.position;
    }

    IEnumerator ResetPlatform()
    {
        float distanceToTarget = Vector3.Distance(startPosition.position, transform.position);
        float time = distanceToTarget / distanceBetweenPositions * timeToReachDestination;
        Vector3 initPos = transform.position;
        float timer = 0f;
        while (Mathf.Abs(distanceToTarget) > 0.1f)
        {
            distanceToTarget = Vector3.Distance(finalPlatformPosition.position, transform.position);
            transform.position = Vector3.Lerp(initPos, startPosition.position, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        //transform.position = startPosition.position;

    }





}
