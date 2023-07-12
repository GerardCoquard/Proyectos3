using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObject : MonoBehaviour
{
    private List<PusheableObject> pushObjects = new List<PusheableObject>();
    private void OnEnable() {
        GetComponent<MoveObject>().OnStart.AddListener(InterpolationOff);
        GetComponent<MoveObject>().OnFinish.AddListener(InterpolationOn);
    }
    private void OnDisable() {
        GetComponent<MoveObject>().OnStart.RemoveListener(InterpolationOff);
        GetComponent<MoveObject>().OnFinish.RemoveListener(InterpolationOn);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CharacterController") return;
        if (other.tag == "Player") return;
        if (other.isTrigger) return;
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pusheableObject != null)
        {
            if (pushObjects.Contains(pusheableObject)) return;
            pusheableObject.transform.SetParent(transform);
            pushObjects.Add(pusheableObject);
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CharacterController") return;
        if (other.tag == "Player") return;
        if (other.isTrigger) return;

        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pusheableObject != null)
        {
            if (!pushObjects.Contains(pusheableObject)) return;
            pusheableObject.transform.SetParent(null);
            pushObjects.Remove(pusheableObject);
        }
    }
    public bool HasObjects()
    {
        List<PusheableObject> temp = new List<PusheableObject>();
        foreach (PusheableObject item in pushObjects)
        {
            if(item == null) temp.Add(item);
        }
        foreach (PusheableObject item in temp)
        {
            pushObjects.Remove(item);
        }
        return pushObjects.Count > 0;
    }

    public void InterpolationOn()
    {
        foreach (PusheableObject item in pushObjects)
        {
            if(item==null) continue;
            if(item.rb == null) continue;
            item.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            item.canBePushed = true;
        }
        
    }
    public void InterpolationOff()
    {
        foreach (PusheableObject item in pushObjects)
        {
            if(item==null) continue;
            if(item.rb == null) continue;
            item.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            item.canBePushed = false;
            if(PlayerController.instance.currentObjectPushing == item) PlayerController.instance.StopPushing();
        }
    }
   
}
