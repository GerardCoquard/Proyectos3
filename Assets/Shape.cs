using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    ShapeType shape;
    private void Start() {
        switch (GetComponent<Collider>())
        {
            case BoxCollider:
            shape = ShapeType.Box;
            break;

            case SphereCollider:
            shape = ShapeType.Sphere;
            break;

            case CapsuleCollider:
            shape = ShapeType.Capsule;
            break;

            default:
            Debug.Log(gameObject.name + " doesn't have an accepted Collider type");
            break;
        }
    }
    
}
public enum ShapeType
{
    Box,
    Sphere,
    Capsule
}
