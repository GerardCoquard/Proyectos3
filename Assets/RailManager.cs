using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public Transform firstLimitPos;
    public Transform lastLimitPos;

    public Transform yReference;

    public Rail newRail;
    public Rail newAuxiliarRail;

    private void Start()
    {
        newRail = GetComponent<Rail>();
    }
}
