using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public Transform firstLimitPos;
    public Transform lastLimitPos;

    public Transform yReference;

    public Rail bottomRail;
    public Rail topRail;

    public Transform newZLimit;

    public void SetNewRails()
    {
        CameraController.instance.ChangeLimits(firstLimitPos, lastLimitPos, newZLimit.position.z);
        CameraController.instance.ChangeRails(bottomRail, topRail, yReference);
    }

}
