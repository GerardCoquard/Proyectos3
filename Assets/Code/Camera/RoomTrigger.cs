using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTrigger : MonoBehaviour
{
    public UnityEvent onRoomChanged;
    public List<TriggerEvent> events;
    public BoxCollider trigger;
    public BoxCollider cameraBox;
    public float extraHeight;
    public float extraDepth;
    public Transform spawnPoint;
    public int roomID;
    public void RoomOnTriggerEnter(Collider other)
    {
        if (other.tag == "CharacterController") return;
        if (other.tag == "Player" || other.tag == "Book") ChangeRoom();
    }

    private void DoEvents()
    {
        foreach (TriggerEvent triggerEvent in events)
        {
            triggerEvent.DoMyEvent();
        }
    }
    public void ChangeRoom()
    {
        if (spawnPoint != null) Save();
        DoEvents();
        CameraController.instance.ChangeRoom(cameraBox, extraHeight, extraDepth);
        onRoomChanged?.Invoke();
    }

    private void Save()
    {
        GameSaveManager.instance.SetCurrentRoom(roomID);
        GameSaveManager.instance.EnableLevels();
        GameSaveManager.instance.UnenableLevels();
    }
}



