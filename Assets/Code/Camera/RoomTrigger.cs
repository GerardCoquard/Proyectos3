using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public UnityEvent onRoomChanged;

    public Transform firstLimitPos;
    public Transform lastLimitPos;

    public Transform firstLimitRot;
    public Transform lastLimitRot;
    
    private Collider myCollider;


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
        CameraController.instance.ChangeLimits(firstLimitPos, lastLimitPos);
        onRoomChanged?.Invoke();
        
    }

}



