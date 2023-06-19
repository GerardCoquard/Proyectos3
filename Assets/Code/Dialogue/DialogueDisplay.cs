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
    [SerializeField] public GameObject uiRenderer;
    [SerializeField] RectTransform textBox;
    private Vector3 initialUiPosition;
    [SerializeField] TextMeshProUGUI dialogueText;
    private DialogueNode startNode;
    private Transform interactablePos;
    private DialogueNode currentNode;
    [SerializeField] float defaultTypeSpeed;
    [SerializeField] float fastTypeSpeed;
    private float currentTypeSpeed;
    [SerializeField] string dialogueID;
    public bool isTextFinished;
    private bool onAnimation;
    private DialogueEventHandler currentEventHandler;

    Vector3 initialScale;
    Vector3 finalScale;
    public float timeToReachScale;
    public float finalScaleMultiplier;
    float distanceBetweenScales;

    public UnityEvent onEndEvent;
    private DIALOGUE_STATE currentState = DIALOGUE_STATE.DEFAULT;

    public static DialogueDisplay instance;

    [SerializeField] TextMeshProUGUI emisorName;
    [SerializeField] TextMeshProUGUI emisorText;

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
        initialUiPosition = new Vector3(uiRenderer.transform.localPosition.x, 0, uiRenderer.transform.localPosition.z);

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

    private void SetEmisorName()
    {
        if (emisorName.text == currentNode.emisor) return;
        emisorName.text = currentNode.emisor;
    }
    public void StartDialogue()
    {
        currentEventHandler.DisableInteractParticles();
        StartCoroutine(WaitToLand());
        SetEmisorName();
        PlayerController.instance.BlockPlayerInputs(false);
        PlayerController.instance.GetAnimator().SetBool("isMoving", false);
        dialogueRender.SetActive(true);
        dialogueText.text = "";
        currentNode = startNode;
        currentState = DIALOGUE_STATE.DEFAULT;
        currentTypeSpeed = defaultTypeSpeed;
    }
    IEnumerator WaitToLand()
    {
        while (PlayerController.instance.GetIsJumping())
        {
            yield return null;
        }
        PlayerController.instance.characterController.enabled = false;
    }
    public void SetStartNode(DialogueNode startNode)
    {
        this.startNode = startNode;
    }

    public void SetEventHandler(DialogueEventHandler handler)
    {
        this.currentEventHandler = handler;
    }
    public void SetInteractablePos(Transform pos)
    {
        interactablePos = pos;
    }


    IEnumerator Type()
    {
        onAnimation = false;
        foreach (char letter in LocalizationManager.GetLocalizedValue(currentNode.textID).ToCharArray())
        {

            dialogueText.text += letter;
            AudioManager.Play("textSound").Pitch(1 + UnityEngine.Random.Range(-0.3f, 0f)).Volume(0.3f);
            yield return new WaitForSeconds(currentTypeSpeed);
        }
        isTextFinished = true;

    }


    private void NextSentence()
    {
        if (currentNode.TargetNode != null)
        {
            currentNode = currentNode.TargetNode;
            SetEmisorName();
            dialogueText.text = "";
            dialogueRender.transform.localScale = initialScale;
        }
        else
        {
            EndDialogue();
        }
    }
    private void EndDialogue()
    {
        BookMovement.instance.DialogueEnded();
        PlayerController.instance.BlockPlayerInputs(true);
        PlayerController.instance.characterController.enabled = true;
        onEndEvent?.Invoke();
        currentEventHandler?.DoEndAnimation();
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

