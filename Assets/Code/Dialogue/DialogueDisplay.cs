using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] GameObject dialogueRender;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] DialogueNode startNode;
    [SerializeField] Transform myPosition;
    private DialogueNode currentNode;
    [SerializeField] float defaultTypeSpeed;
    [SerializeField] float fastTypeSpeed;
    private float currentTypeSpeed;
    [SerializeField] string dialogueID;
    private bool isTextFinished;

    private Interactable myInteractable;

    public static Action<string> OnEndDialogue;
    private DIALOGUE_STATE currentState = DIALOGUE_STATE.DEFAULT;

    private void Start()
    {
        myInteractable = GetComponent<Interactable>();
    }
    private void OnEnable()
    {
        InputManager.GetAction("Push").action += Interact;
    }

    private void OnDisable()
    {
        InputManager.GetAction("Push").action -= Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isTextFinished)
            {
                currentTypeSpeed = defaultTypeSpeed;
                currentState = DIALOGUE_STATE.DEFAULT;
                NextSentence();
            }
            else
            {
                switch (currentState)
                {
                    case DIALOGUE_STATE.DEFAULT:
                        currentTypeSpeed = fastTypeSpeed;
                        currentState = DIALOGUE_STATE.FAST;
                        break;
                    case DIALOGUE_STATE.FAST:
                        ShowFullText();
                        currentState = DIALOGUE_STATE.SKIP;
                        break;
                }
            }
        }
    }

    public void StartDialogue()
    {
        myInteractable.enabled = false;
        dialogueText.text = "";
        currentNode = startNode;
        currentState = DIALOGUE_STATE.DEFAULT;
        currentTypeSpeed = defaultTypeSpeed;
        dialogueRender.transform.position = currentNode.emisor == SPEAKER.ME ? myPosition.position : PlayerController.instance.transform.position;
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        isTextFinished = false;
        foreach (char letter in currentNode.Text.ToCharArray())
        {
            
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentTypeSpeed);
        }
        isTextFinished = true;
    }

    private void NextSentence()
    {
        if (currentNode.TargetNode != null)
        {
            currentNode = currentNode.TargetNode;
            dialogueText.text = "";
            dialogueRender.transform.position = currentNode.emisor == SPEAKER.ME ? myPosition.position : PlayerController.instance.transform.position;
            StartCoroutine(Type());
        }
        else
        {
            EndDialogue();
        }
    }
    private void EndDialogue()
    {
        myInteractable.enabled = true;
        OnEndDialogue?.Invoke(dialogueID);
        dialogueRender.SetActive(false);
    }

    private void ShowFullText()
    {
        StopAllCoroutines();
        isTextFinished = true;
        dialogueText.text = currentNode.Text;
    }
}


public enum DIALOGUE_STATE
{
    DEFAULT,
    FAST,
    SKIP
}

