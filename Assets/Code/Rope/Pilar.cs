using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pilar : MonoBehaviour
{
    public UnityEvent OnActivate;
    public UnityEvent OnDisable;
    public PilarManager pilar;
    int collisions;
    bool locked;
    public void OnCollision()
    {
        if(locked) return;
        if(collisions==0)
        {
            pilar.AddPilar(this);
            OnActivate?.Invoke();
        }
        collisions++;
    }
    public void OnExitCollision()
    {
        if(locked) return;
        collisions--;
        if(collisions==0)
        {
            pilar.RemovePilar(this);
            OnDisable?.Invoke();
        }
    }
    public void SetLocked(bool state)
    {
        locked = state;
    }
}
