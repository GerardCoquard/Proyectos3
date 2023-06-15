using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PressurePlateFixed : MonoBehaviour
{
    public int numberLinked;
    public UnityEvent OnPressed;
    public UnityEvent OnUnpressed;
    List<GameObject> onTop = new List<GameObject>();
    bool locked;
    Animator anim;
    bool pressed;
    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }
    public bool IsPressed()
    {
        return pressed;
    }
    private void OnTriggerEnter(Collider other) {
        if(locked) return;
        if (other.tag == "CharacterController") return;
        if (onTop.Count == 0) anim.SetBool("Pressed",true);
        onTop.Add(other.gameObject);
        FixedPlateObject objectPressing = other.GetComponent<FixedPlateObject>();
        if(objectPressing!=null)
        {
            if(objectPressing.number == numberLinked)
            {
                pressed = true;
                OnPressed?.Invoke();
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(locked) return;
        if (other.tag == "CharacterController") return;
        if (onTop.Contains(other.gameObject))
        {
            onTop.Remove(other.gameObject);
            if(onTop.Count == 0) anim.SetBool("Pressed",false);
        }
        FixedPlateObject objectPressing = other.GetComponent<FixedPlateObject>();
        if(objectPressing!=null)
        {
            if(objectPressing.number == numberLinked)
            {
                OnUnpressed?.Invoke();
                pressed = false;
            }
        }
    }
    public void SetLocked(bool state)
    {
        locked = state;
    }
    public void SetAnimPressed(bool state)
    {
        anim.SetBool("Pressed",state);
    }
}

