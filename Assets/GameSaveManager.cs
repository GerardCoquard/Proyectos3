using System.Collections.Generic;
using UnityEngine;


public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    private int currentRoom;
    public List<RoomTrigger> roomTriggers;

    public List<GameObject> levels;
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

    public void EnableLevels()
    {
       
        if (levels == null) return;
        if (currentRoom + 1 < levels.Count) levels[currentRoom + 1].SetActive(true);
        if (currentRoom - 1 >= 0) levels[currentRoom - 1].SetActive(true);
    }

    public void UnenableLevels()
    {
        Debug.Log("IN");
        int levelForward = currentRoom + 2;
        int levelBackward = currentRoom - 2;
        if (levels == null) return;
        if(levelForward <= levels.Count)
        {
            for (int i = levelForward; i < levels.Count; i++)
            {
                levels[i].SetActive(false);
            }
        }

        if(levelBackward >= 0)
        {
            for (int i = levelBackward; i >= 0; i--)
            {
                levels[i].SetActive(false);
            }
        }

    }


   

    private void Load()
    {
        currentRoom = DataManager.Load<int>("roomID");
        UnenableLevels();
    }

    private void Save()
    {
        DataManager.Save("roomID", currentRoom);
    }
}
    