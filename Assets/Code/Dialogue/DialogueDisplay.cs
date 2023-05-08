using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] GameObject dialogueRender;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] DialogueNode startNode;
    [SerializeField] List<Transform> positions = new List<Transform>();
    private DialogueNode currentNode;
    [SerializeField] float defaultTypeSpeed;
    [SerializeField] float fastTypeSpeed;
    private float currentTypeSpeed;
    [SerializeField] string dialogueID;
    private bool isTextFinished;

    public static Action<string> OnEndDialogue;
    private DIALOGUE_STATE currentState = DIALOGUE_STATE.DEFAULT;
    private void OnEnable()
    {
        currentNode = startNode;
        currentTypeSpeed = defaultTypeSpeed;
        dialogueRender.transform.position = positions[currentNode.emisor].position;
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
            dialogueRender.transform.position = positions[currentNode.emisor].position;
            StartCoroutine(Type());
        }
        else
        {
            EndDialogue();
        }
    }


    private void EndDialogue()
    {
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

