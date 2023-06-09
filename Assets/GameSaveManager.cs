using System.Collections.Generic;
using UnityEngine;


public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    private int currentRoom;
    public List<RoomTrigger> roomTriggers;

    public Transform GetSpawnPoint()
    {
        return roomTriggers[currentRoom].spawnPoint;
    }
    public RoomTrigger GetCurrentRoom()
    {
         return roomTriggers[currentRoom];
    }

    public void SetCurrentRoom(int id)
    {
        currentRoom = id;
        Save();
    }

    private void Awake()
    {
        Load();
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

   

    private void Load()
    {
        currentRoom = DataManager.Load<int>("roomID");
    }

    private void Save()
    {
        DataManager.Save("roomID", currentRoom);
    }
}
    