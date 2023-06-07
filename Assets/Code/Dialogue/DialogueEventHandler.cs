using System.Collections;
using UnityEngine;

public class DialogueEventHandler : MonoBehaviour
{
    public DialogueNode startNode;
    public Transform otherTransform;
    public float dialogueStartDelay;
    public Animation actorAnimation;
    public AnimationClip actorStartAnimation;
    public AnimationClip actorEndAnimation;
    public void StartDialogueEvent()
    {
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (actorStartAnimation != null) actorAnimation.PlayQueued(actorStartAnimation.name);
        yield return new WaitForSeconds(dialogueStartDelay);
        WorldScreenUI.instance.SetDialogue(startNode, otherTransform);
        if (actorEndAnimation != null && DialogueDisplay.instance.isTextFinished) { actorAnimation.PlayQueued(actorEndAnimation.name); Debug.Log("IN"); }
    }

   

}

