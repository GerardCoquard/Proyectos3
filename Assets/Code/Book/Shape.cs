using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    LayerMask layer;
    public Collider shapeCollider;
    private void OnEnable() {
        Book.OnBookStateChanged += SetOutline;
    }
    private void OnDisable() {
        Book.OnBookStateChanged -= SetOutline;
    }
    private void Start() {
        if(shapeCollider==null) shapeCollider = GetComponent<Collider>();
        Unselect();
    }
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
    void SetOutline(bool state)
    {
        if(state) gameObject.layer = LayerMask.NameToLayer("Outline");
        else gameObject.layer = LayerMask.NameToLayer("Pusheable");
    }

    public void SetSelected()
    {

    }
    public void Unselect()
    {

    }
}
