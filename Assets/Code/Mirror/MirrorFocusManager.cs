using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFocusManager : MonoBehaviour
{
    public static MirrorFocusManager instance;
    public List<PusheableObject> mirrors;
    private void Awake() {
        if(instance == null) instance = this;
        else Destroy(this);
    }
    public void SetFocus(Transform focus)
    {
        foreach (PusheableObject item in mirrors)
        {
            item.focus = focus;
        }
    }

    public void AddMirror(PusheableObject push)
    {
        mirrors.Add(push);
    }
    public void RemoveMirror()
    {
        mirrors.RemoveAt(mirrors.Count-1);
    }
}
