using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public GameObject interactVisuals;
    public UnityEvent interactEvent;

    private void Start()
    {
        interactVisuals.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactVisuals.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (InputManager.GetAction("Push").context.WasPerformedThisFrame())
            {
                interactEvent?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactVisuals.SetActive(false);
        }
    }

}





