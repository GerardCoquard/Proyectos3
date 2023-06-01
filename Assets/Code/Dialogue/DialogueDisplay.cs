using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] public GameObject dialogueRender;
    [SerializeField] TextMeshProUGUI dialogueText;
    private DialogueNode startNode;
    private Transform interactablePos;
    private DialogueNode currentNode;
    [SerializeField] float defaultTypeSpeed;
    [SerializeField] float fastTypeSpeed;
    private float currentTypeSpeed;
    [SerializeField] string dialogueID;
    private bool isTextFinished;
    private bool onAnimation;

    Vector3 initialScale;
    Vector3 finalScale;
    public float timeToReachScale;
    public float finalScaleMultiplier;
    float distanceBetweenScales;

    public UnityEvent onEndEvent;
    private DIALOGUE_STATE currentState = DIALOGUE_STATE.DEFAULT;

    public static DialogueDisplay instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);

        dialogueRender.SetActive(false);
        initialScale = dialogueRender.transform.localScale;
        finalScale = initialScale * finalScaleMultiplier;
        distanceBetweenScales = Vector3.Distance(initialScale, finalScale);
    }
    private void Update()
    {

        dialogueRender.transform.position = currentNode.emisor == SPEAKER.ME ? WorldScreenUI.instance.WorldPosToScreen(interactablePos.position) : WorldScreenUI.instance.WorldPosToScreen(Book.instance.dialoguePosition.position);

    }
    private void OnEnable()
    {
        InputManager.GetAction("Push").action += Interact;
        InputManager.GetAction("ExitDialogue").action += End;
    }

    private void OnDisable()
    {
        InputManager.GetAction("Push").action -= Interact;
        InputManager.GetAction("ExitDialogue").action -= End;
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
            else if (!onAnimation)
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
    private void End(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EndDialogue();
        }
    }

    public void StartDialogue()
    {
        PlayerController.instance.characterController.enabled = false;
        PlayerController.instance.animatorController.SetBool("isMoving", false);
        dialogueRender.SetActive(true);
        dialogueText.text = "";
        currentNode = startNode;
        currentState = DIALOGUE_STATE.DEFAULT;
        currentTypeSpeed = defaultTypeSpeed;
        StartCoroutine(ScaleCoroutine());
    }

    public void SetStartNode(DialogueNode startNode)
    {
        this.startNode = startNode;
    }

    public void SetInteractablePos(Transform pos)
    {
        interactablePos = pos;
    }

    IEnumerator ScaleCoroutine()
    {
        isTextFinished = false;
        onAnimation = true;
        float distanceToScale = Vector3.Distance(dialogueRender.transform.localScale, finalScale * 1.5f);
        float time = distanceToScale / distanceBetweenScales * timeToReachScale;
        Vector3 _initScale = dialogueRender.transform.localScale;
        float timer = 0f;
        while (timer < time)
        {
            dialogueRender.transform.localScale = Vector3.Lerp(_initScale, finalScale * 1.5f, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ScaleBack());
    }

    IEnumerator ScaleBack()
    {
        
        float distanceToScale = Vector3.Distance(dialogueRender.transform.localScale, finalScale);
        float time = distanceToScale / distanceBetweenScales * timeToReachScale;
        Vector3 _initScale = dialogueRender.transform.localScale;
        float timer = 0f;
        while (timer < time)
        {
            dialogueRender.transform.localScale = Vector3.Lerp(_initScale, finalScale, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        dialogueRender.transform.localScale = finalScale;
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        onAnimation = false;
        foreach (char letter in LocalizationManager.GetLocalizedValue(currentNode.textID).ToCharArray())
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
            dialogueRender.transform.localScale = initialScale;
            StartCoroutine(ScaleCoroutine());
        }
        else
        {
            EndDialogue();
        }
    }
    private void EndDialogue()
    {
        PlayerController.instance.characterController.enabled = true;
        onEndEvent?.Invoke();
        dialogueRender.gameObject.SetActive(false);
        this.enabled = false;
    }

    private void ShowFullText()
    {
        StopAllCoroutines();
        isTextFinished = true;
        dialogueText.text = LocalizationManager.GetLocalizedValue(currentNode.textID);
    }
}


public enum DIALOGUE_STATE
{
    DEFAULT,
    FAST,
    SKIP
}

