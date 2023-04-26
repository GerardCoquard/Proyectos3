using System.Collections;
using UnityEngine;

public class SwitchablePlatformGym : MonoBehaviour
{
    [SerializeField] private Transform finalPlatformPosition;
    [SerializeField] private float speed;


    public void SetPlatformToNewPosition()
    {
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        float distanceBetweenPositions = (finalPlatformPosition.position - transform.position).magnitude;
        while (Mathf.Abs(distanceBetweenPositions) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, finalPlatformPosition.position, speed * Time.deltaTime);
            yield return null;
        }
    }

}
