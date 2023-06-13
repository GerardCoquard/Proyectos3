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
    public GameObject interactableParticles;
    public void StartDialogueEvent()
    {
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (actorStartAnimation != null) actorAnimation.PlayQueued(actorStartAnimation.name);
        Book.instance.ResetBookGraphics();
        interactableParticles.SetActive(false);
        BookMovement.instance.DialogueStarted();
        yield return new WaitForSeconds(dialogueStartDelay);
        WorldScreenUI.instance.SetDialogue(startNode, otherTransform, this);
        
    }

    public void DisableInteractParticles()
    {
        if (interactableParticles != null) interactableParticles.SetActive(false);
    }

    public void EnableInteractParticles()
    {
        if (interactableParticles != null) interactableParticles.SetActive(true);
    }
    public void DoEndAnimation()
    {
        if (actorEndAnimation != null) actorAnimation.PlayQueued(actorEndAnimation.name);
    }



}

