using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraChild : MonoBehaviour
{
    RoomTrigger room;
    private void Start()
    {
        room = GetComponentInParent<RoomTrigger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CharacterController") return;
        if (other.tag == "Player")
        {

            room.RoomOnTriggerEnter(other);
        }
    }
}
