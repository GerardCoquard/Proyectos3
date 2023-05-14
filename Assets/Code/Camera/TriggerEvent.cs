using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent eventToDo;
    private bool activated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            eventToDo?.Invoke();
            activated = true;
        }
    }
}
