using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheckerZone : MonoBehaviour
{
    List<PusheableObject> pusheables = new List<PusheableObject>();
    public List<Transform> validPositions;
    public float timeToMove;
    private void OnTriggerEnter(Collider other) {
        PusheableObject push = other.GetComponentInParent<PusheableObject>();
        if(push != null)
        {
            if(!pusheables.Contains(push)) pusheables.Add(push);
        }
    }
    private void OnTriggerExit(Collider other) {
        PusheableObject push = other.GetComponentInParent<PusheableObject>();
        if(push != null)
        {
            if(pusheables.Contains(push)) pusheables.Remove(push);
        }
    }
    public void MoveStuff()
    {
        List<PusheableObject> temp = new List<PusheableObject>();
        foreach (PusheableObject item in pusheables)
        {
            if(item == null) temp.Add(item);
        }
        foreach (PusheableObject item in temp)
        {
            pusheables.Remove(item);
        }

        foreach (PusheableObject item in pusheables)
        {
            MoveOut(item);
        }
    }
    void MoveOut(PusheableObject push)
    {
        int indx = 0;
        float distance = Vector3.Distance(push.transform.position,validPositions[0].position);
        for (int i = 0; i < validPositions.Count; i++)
        {
            if(Vector3.Distance(push.transform.position,validPositions[i].position) < distance)
            {
                distance = Vector3.Distance(push.transform.position,validPositions[i].position);
                indx = i;
            }
        }
        StartCoroutine(Move(push,validPositions[indx]));
        validPositions.RemoveAt(indx);
    }
    IEnumerator Move(PusheableObject push,Transform finalPos)
    {
        push.gameObject.AddComponent<MoveObject>();
        MoveObject mover = push.GetComponent<MoveObject>();
        mover.ChangeParams(finalPos,timeToMove);
        mover.Move();
        yield return new WaitForSeconds(timeToMove);
        yield return new WaitForEndOfFrame();
        Destroy(mover);
    }
}
