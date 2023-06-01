using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    public DialogueNode startNode;
    public Transform otherTransform;

    public void StartDialogueEvent()
    {
        WorldScreenUI.instance.SetDialogue(startNode,otherTransform);

    }


}

