using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    Transform target;
    public Transform mirror;
    public float offsetX;

    void Start()
    {
        target = Camera.main.transform;
    }
    void Update()
    {
       Vector3 localTarget = mirror.InverseTransformPoint(target.position - new Vector3(0,offsetX,0));     
       Vector3 lookAtMirror = mirror.TransformPoint(new Vector3(-localTarget.x, localTarget.y, localTarget.z));
       transform.LookAt(lookAtMirror);
        
    }
}
