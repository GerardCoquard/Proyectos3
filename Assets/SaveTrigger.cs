using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Save();
        }
    }

    private void Save()
    {
        DataManager.Save("playerPosition", PlayerController.instance.transform.position);
        DataManager.Save("cameraPosition", CameraController.instance.transform.position);
    }
}
