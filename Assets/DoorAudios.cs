using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudios : MonoBehaviour
{
   
   public void DoorSound()
    {
        AudioManager.Play("closingDoor" + Random.Range(1, 6).ToString()).Volume(0.5f);
    }
}
