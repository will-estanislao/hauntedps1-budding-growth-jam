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

    private Queue<string> sentences;

    private int dialogeCount;

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

        sentences = new Queue<string>();
    }

    public void StartDialogue(string[] dialogueArray, string name)
    {
        Main.Instance.InConversation();
        // When dialogue starts- evoke event in main that disables ui and focuses on click events to moving dialogue
        //Debug.Log("Current Convo: " + dialogue.badEndDialogue);
        DialogueName.text = name;
        DialogBox.visible = true;

        

        sentences.Clear();
        // Plant responsible for what dialogue gets fed
        foreach(string sentence in dialogueArray)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    // For singular dialogue
    public void StartDialogue(string[] dialogueArray, int startPos, string name)
    {
        Main.Instance.InConversation();
        DialogueName.text = name;
        DialogBox.visible = true;
        sentences.Clear();
        sentences.Enqueue(dialogueArray[startPos]);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            Main.Instance.EndConversation();
            return;
        }

        string sentence = sentences.Dequeue();
        DialogueText.text = sentence;
    }

    /*
    public bool IsLastSentence()
    {
        
    }
    */

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
