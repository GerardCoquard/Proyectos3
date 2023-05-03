using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LimitTrigger : MonoBehaviour
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
        RailMover.instance.ChangeLimits(firstLimitPos, lastLimitPos, firstLimitRot, lastLimitRot);
        onRoomChanged?.Invoke();
        
    }

}



