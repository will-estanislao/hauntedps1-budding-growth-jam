using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueController: MonoBehaviour
{
    public static DialogueController instance;

    private VisualElement dialogueBox;

    public VisualElement DialogBox { get; set; }

    private Label dialogueText;

    public Label DialogueText { get; set; }

    private Label dialogueName;

    public Label DialogueName { get; set; }

    // Events for showing dialog
    public static event Action OnDialogStarted;
    public static event Action OnDialogEnded;

    bool skipLineTriggered;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowDialogue(string[] dialogue, int startPosition, string name)
    {
        DialogueName.text = name;

        //DialogueText.text = dialogue;

        DialogBox.visible = true;
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));

    }

    public void ShowDialogue2(string[] dialogue, int startPosition, string name)
    {
        DialogueName.text = name;

        //DialogueText.text = dialogue;

        DialogBox.visible = true;
        //StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));

    }

    IEnumerator RunDialogue(string[] dialogue, int startPositon)
    {
        skipLineTriggered = false;
        OnDialogStarted?.Invoke();

        for(int i = startPositon; i < dialogue.Length; i++)
        {
            DialogueText.text = dialogue[i];
            while(skipLineTriggered == false)
            {
                // Wait for current line to be skipped
                yield return null;
            }
            skipLineTriggered = false;
        }
        OnDialogEnded?.Invoke();
        DialogBox.visible = false;
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void EndDialogue()
    {
        DialogueName.text = null;

        DialogueText.text = null;

        DialogBox.visible = false;
    }

    public void InitializeDialog()
    {
        EndDialogue();
    }

}
