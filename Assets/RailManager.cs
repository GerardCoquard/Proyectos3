using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public BoxCollider box;
    public float extraHeight;
    public float maxAngle;

    public void SetRoomParams()
    {
        //CameraController.instance.ChangeRoom(box, extraHeight, maxAngle);
    }

}
