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
    [SerializeField] List<string> sentencesList = new List<string>();
    private int index;
    [SerializeField] float defaultTypeSpeed;
    [SerializeField] float fastTypeSpeed;
    private float currentTypeSpeed;
    [SerializeField] string dialogueID;
    private bool isTextFinished;

    public static Action<string> OnEndDialogue;
    private DIALOGUE_STATE currentState = DIALOGUE_STATE.DEFAULT;
    private void Start()
    {
        StartCoroutine(Type());
        currentTypeSpeed = defaultTypeSpeed;
    }

    private void OnEnable()
    {
        InputManager.GetAction("Jump").action += Interact;
    }

    private void OnDisable()
    {
        InputManager.GetAction("Jump").action -= Interact;
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

    IEnumerator Type()
    {
        isTextFinished = false;
        foreach (char letter in sentencesList[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentTypeSpeed);
        }
        isTextFinished = true;
    }

    private void NextSentence()
    {

        if (index < sentencesList.Count - 1)
        {
            index++;
            dialogueText.text = "";
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
        dialogueText.text = sentencesList[index].ToString();
    }
}


public enum DIALOGUE_STATE
{
    DEFAULT,
    FAST,
    SKIP
}

