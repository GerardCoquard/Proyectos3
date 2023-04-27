using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerProto : MonoBehaviour
{
    [SerializeField] Transform maxTransform;
    [SerializeField] Transform minTransform;
    [SerializeField] float maxDistanceX;
    [SerializeField] float speed;
    [SerializeField] PlayerController player;
    float yPos;
    float zPos;
    private void Start() {
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

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
                transform.position = new Vector3(transform.position.x, yPos, zPos);
    }
}
