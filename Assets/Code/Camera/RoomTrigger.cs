using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public UnityEvent onRoomChanged;
   
    private Collider myCollider;

    public RailManager railManager;



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
        railManager.SetNewRails();
        onRoomChanged?.Invoke();
        
    }

}



