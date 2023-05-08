using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shape : MonoBehaviour
{
    LayerMask layer;
    public Collider shapeCollider;
    [NonSerialized] public ShapeType type;

    private void OnEnable() {
        Book.OnBookStateChanged += SetOutline;
    }
    private void OnDisable() {
        Book.OnBookStateChanged -= SetOutline;
    }
    private void Start() {
        layer = gameObject.layer;
        if(shapeCollider==null) shapeCollider = GetComponent<Collider>();
        switch (shapeCollider)
        {
            case BoxCollider:
            type = ShapeType.Box;
            break;

            case SphereCollider:
            type = ShapeType.Sphere;
            break;

            case CapsuleCollider:
            type = ShapeType.Capsule;
            break;
        }
        Unselect();
    }
    public void Shift() {
        Book.instance.Shapehift(this,shapeCollider.bounds.extents);
    }
    void SetOutline(bool state)
    {
        if(state) gameObject.layer = LayerMask.NameToLayer("Outline");
        else gameObject.layer = layer;
    }
    public void SetSelected()
    {

    }
    public void Unselect()
    {

    }
}
public enum ShapeType
{
    Box,
    Sphere,
    Capsule
}
