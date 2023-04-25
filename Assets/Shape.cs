using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public Collider shapeCollider;
    public void Shift() {
        if(shapeCollider==null)
        {
            Debug.LogWarning(gameObject.name + " doesn't have a Collider associated");
            return;
        }
        switch (shapeCollider)
        {
            case BoxCollider:
            Vector3 extents = shapeCollider.bounds.extents;
            Book.instance.ShapeshiftBox(this,extents);
            break;

            case SphereCollider:
            float radiusSphere = ((SphereCollider)shapeCollider).radius;
            Book.instance.ShapeshiftSphere(this,radiusSphere);
            break;

            case CapsuleCollider:
            float radiusCapsule = ((CapsuleCollider)shapeCollider).radius;
            float height = ((CapsuleCollider)shapeCollider).height;
            Book.instance.ShapeshiftCapsule(this,radiusCapsule,height);
            break;

            default:
            Debug.Log(gameObject.name + " doesn't have an accepted Collider type");
            break;
        }
    }
}
