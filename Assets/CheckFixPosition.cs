using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFixPosition : MonoBehaviour
{
    public Transform finalDestination;
    public float maxDistance;
    public float timeToReach;
    PusheableObject push;
    private void Start() {
        push = GetComponent<PusheableObject>();
    }
    public void CheckDistance()
    {
        if(Vector3.Distance(transform.position,finalDestination.position) <= maxDistance)
        {
            StartCoroutine(Move());
        }
    }
    IEnumerator Move()
    {
        push.canBePushed = false;
        MoveObject move = gameObject.AddComponent<MoveObject>();
        move.ChangeParams(finalDestination,timeToReach);
        move.Move();
        yield return new WaitForSeconds(timeToReach);
        yield return new WaitForEndOfFrame();
        Destroy(move);
        push.canBePushed = true;
    }
}
