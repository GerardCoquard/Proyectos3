using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public Transform target;
    public Transform mirror;

    void Update()
    {
       Vector3 localTarget = mirror.InverseTransformPoint(target.position);     
       Vector3 lookAtMirror = mirror.TransformPoint(new Vector3(-localTarget.x, localTarget.y, localTarget.z));
       transform.LookAt(lookAtMirror);
        
    }
}
