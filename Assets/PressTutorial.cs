using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressTutorial : MonoBehaviour
{
    public static PressTutorial instance;
    public GameObject holder;
    private void Awake() {
        if(instance == null) instance = this;
        else Destroy(this);
    }
    public void SetTutorial(bool state)
    {
        holder.SetActive(state);
    }

}
