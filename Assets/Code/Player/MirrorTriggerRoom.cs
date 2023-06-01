using UnityEngine;
using System;
using System.Collections;

public class MirrorTriggerRoom : MonoBehaviour
{
    [SerializeField] CopyMovement mirroedPlayer;
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player")
        {
            Debug.Log("IN");
            mirroedPlayer.SetLocked(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("out");
            mirroedPlayer.SetLocked(true);
        }
    }
}

