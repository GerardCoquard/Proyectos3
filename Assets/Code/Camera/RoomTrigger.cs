using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public UnityEvent onRoomChanged;

    public Transform firstLimitPos;
    public Transform lastLimitPos;

    public Transform yReference;

    public Rail newRail;
    public Rail newAuxiliarRail;
    
    private Collider myCollider;

    public Transform newZLimit;


    private void Start()
    {
        myCollider = GetComponent<Collider>();  
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ChangeRoom();
        }
    }

    public void ChangeRoom()
    {
        myCollider.enabled = false;
        CameraController.instance.ChangeLimits(firstLimitPos, lastLimitPos, newZLimit.position.z);
        CameraController.instance.ChangeRails(newRail, newAuxiliarRail, yReference);
        onRoomChanged?.Invoke();
        
    }

}



