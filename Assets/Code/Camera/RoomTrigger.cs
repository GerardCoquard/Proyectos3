using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public UnityEvent onRoomChanged;
    public BoxCollider trigger;
    public BoxCollider cameraBox;
    public float extraHeight;
    public float extraDepth;
    public void RoomOnTriggerEnter(Collider other)
    {
        if (other.tag == "CharacterController") return;
        if (other.tag == "Player") ChangeRoom();
    }
    public void ChangeRoom()
    {
        trigger.enabled = false;
        CameraController.instance.ChangeRoom(cameraBox, extraHeight, extraDepth);
        onRoomChanged?.Invoke();
    }
}



