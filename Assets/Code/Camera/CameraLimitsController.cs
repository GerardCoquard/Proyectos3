using UnityEngine;

public class CameraLimitsController : MonoBehaviour
{
    public RailMover mover;

    public void SetNewLimits(Transform fp, Transform lp, Transform fr, Transform lr)
    {
        mover.firstLimitPosition = fp;
        mover.lastLimitPosition = lp;

        mover.firstLimitRotation = fr;
        mover.lastLimitRotation= lr;
    }
}



