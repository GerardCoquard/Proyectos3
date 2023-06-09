using System.Collections;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] Transform spawnPointThisRoom;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Save();
        }
    }

    private void Save()
    {
        DataManager.Save("playerPosition", spawnPointThisRoom.position);
        
    }
}
    