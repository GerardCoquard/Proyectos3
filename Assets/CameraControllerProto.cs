using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerProto : MonoBehaviour
{
    [SerializeField] Transform maxTransform;
    [SerializeField] Transform minTransform;
    [SerializeField] Transform myTransform;
    [SerializeField] float maxDistanceX;
    [SerializeField] float speed;
    [SerializeField] PlayerController player;

    private void Update()
    {
        float currentDistance = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(currentDistance) >= maxDistanceX)
        {
            if (transform.position.x > maxTransform.position.x && transform.position.x < minTransform.position.x)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
                transform.position = new Vector3(transform.position.x, 8.46f, -12.44f);
    }
}
