using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PressurePlatesPuzzleController: MonoBehaviour
{
    [SerializeField] List<PressurePlateFixed> plates;
    [SerializeField] UnityEvent eventWhenComplete;
    private bool eventInvoked = false;


    public void CheckIfComplete()
    {
        foreach (PressurePlateFixed plate in plates)
        {
            if (!plate.IsPressed()) return;
        }
        if (!eventInvoked)
        {
            eventWhenComplete?.Invoke();
            eventInvoked = true;
        } 
    }

}

