using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithLerp : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    void Update()
    {
        MoveLerp();
    }

    private void MoveLerp()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
    }
}
