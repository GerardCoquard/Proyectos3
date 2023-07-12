using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjects : MonoBehaviour
{
    List<PusheableObject> pusheables;
    private void Start() {
        pusheables = new List<PusheableObject>(GetComponentsInChildren<PusheableObject>());
    }
    public void SetFocus(Transform newFocus)
    {
        foreach (PusheableObject item in pusheables)
        {
            item.focus = newFocus;
        }
    }
}
