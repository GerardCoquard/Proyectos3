using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent eventToDo;
    public UnityEvent eventToDoWithTime;
    public float timeToEventWithTimer;
    private bool activated;
    public bool needInput;
    private bool inCollider = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !activated)
        {
            inCollider = true;
            if (needInput) return;
            if (eventToDoWithTime != null)
            {
                StartCoroutine(EventTimer());
            }
            activated = true;
            eventToDo?.Invoke();
        }
    }

    IEnumerator EventTimer()
    {
        yield return new WaitForSeconds(timeToEventWithTimer);
        eventToDoWithTime?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            inCollider = false;
        }
    }

    private void Update()
    {
        if (!needInput) return;
        if (InputManager.GetAction("Push").context.WasPerformedThisFrame() && !activated && inCollider)
        {
            activated = true;
            eventToDo?.Invoke();
            if(eventToDoWithTime != null)
            {
                StartCoroutine(EventTimer());
            }
           
        }
    }
}
