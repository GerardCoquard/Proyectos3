using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSensor : MonoBehaviour
{
    int _overlaps;
    public bool IsOverlapping()
    {
        return _overlaps > 0;
    }
    private void OnTriggerEnter(Collider other) {
        _overlaps++;
        Debug.Log("Overlaped");
    }
    private void OnTriggerExit(Collider other) {
        _overlaps--;
        Debug.Log("Stopped Overlaping");
    }
}
