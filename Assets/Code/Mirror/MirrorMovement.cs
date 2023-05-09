using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform mirror;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Vector3 localTarget = mirror.InverseTransformPoint(target.position);     
       Vector3 lookAtMirror = mirror.TransformPoint(new Vector3(-localTarget.x, localTarget.y, localTarget.z));
       transform.LookAt(lookAtMirror);
        
    }
}
