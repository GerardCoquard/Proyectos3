using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    public DialogueNode startNode;

    public void StartDialogueEvent()
    {
        WorldScreenUI.instance.SetDialogue(startNode, Book.instance.dialoguePosition);
    }

}

