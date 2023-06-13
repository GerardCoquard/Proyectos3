using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PressurePlatesPuzzleController: MonoBehaviour
{
    [SerializeField] List<PressurePlateFixed> plates;
    [SerializeField] UnityEvent eventWhenComplete;


    public void CheckIfComplete()
    {
        foreach (PressurePlateFixed plate in plates)
        {
            if (!plate.IsPressed()) return;
        }
        eventWhenComplete?.Invoke();
    }

}

