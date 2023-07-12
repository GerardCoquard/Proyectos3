using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudios : MonoBehaviour
{
    private bool triggered = false;
   public void DoorSound()
    {
        if (triggered) return;
        AudioManager.Play("closingDoor" + Random.Range(1, 6).ToString()).Volume(0.5f);
        triggered = true;
    }
}
