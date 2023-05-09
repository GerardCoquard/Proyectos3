using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public GameObject interactVisuals;
    private DialogueDisplay dialogueDisplay;
   

    private bool canInteract;

    private void Start()
    {
        dialogueDisplay = GetComponent<DialogueDisplay>();
        interactVisuals.SetActive(false);
    }

    private void Update()
    {
        if(canInteract && InputManager.GetAction("Push").context.WasPerformedThisFrame())
        {
            canInteract = false;
            dialogueDisplay.StartDialogue();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactVisuals.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactVisuals.SetActive(false);
            canInteract = false;
        }
    }

}





