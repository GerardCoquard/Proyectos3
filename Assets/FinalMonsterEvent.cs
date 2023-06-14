using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMonsterEvent : MonoBehaviour
{
    public List<EventMonsterMirror> pilars;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) ThrowLasers();
    }
    public void ThrowLasers()
    {
        foreach (EventMonsterMirror pilar in pilars)
        {
            pilar.ThrowToMonster();
        }
    }
}
