using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMonsterEvent : MonoBehaviour
{
    public List<EventMonsterMirror> pilars;

   
    public void ThrowLasers()
    {
        foreach (EventMonsterMirror pilar in pilars)
        {
            pilar.StartMonsterEvent();
        }
    }
}
