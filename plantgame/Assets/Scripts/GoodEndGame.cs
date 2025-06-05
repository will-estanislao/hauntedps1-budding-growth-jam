using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GoodEndGame : MonoBehaviour
{
    [SerializeField]
    DialogueAsset endDialogue;

    // UI
    private UIDocument doc;
    private VisualElement screen;
    private VisualElement dialog;

    private int dialoguePos;

    private bool isEnd;


    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        screen = doc.rootVisualElement.Q(name: "screen");
        dialog = doc.rootVisualElement.Q<VisualElement>(name: "dialogBox");
        //screen.RegisterCallback<ClickEvent>(EndGame);
    }

    private void Start()
    {
        StopAllCoroutines();
        // Dialog box
        DialogueController.instance.DialogBox = dialog;
        DialogueController.instance.DialogueName = DialogueController.instance.DialogBox.Q<Label>(name: "name");
        DialogueController.instance.DialogueText = DialogueController.instance.DialogBox.Q<Label>(name: "dialogText");

        DialogueController.instance.StartDialogue(endDialogue.dialogue, 0, "");
    }

    private void Update()
    {
        if (endDialogue.dialogue.Length - 1 <= dialoguePos)
        {
            if(!AudioController.Instance.IsItPlaying() && !AudioController.Instance.isItPlaying)
            {
                StartCoroutine(AudioController.Instance.PlayEndAudio());
                //AudioController.Instance.PlayEndAudio2();
                AudioController.Instance.isItPlaying = true;
            }

            StartCoroutine(EndGame());
            
        }
        else
        {
            OnClick();
        }
        
    }

    public void DialogueNext()
    {
        ++dialoguePos;
        if (dialoguePos % 2 == 0)
       {
            DialogueController.instance.StartDialogue(endDialogue.dialogue, dialoguePos, "Player");
       }
       else
       {
            DialogueController.instance.StartDialogue(endDialogue.dialogue, dialoguePos, "Cop");
       }
        
    }

    public void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {

            DialogueNext();

        }
    }

    public IEnumerator EndGame()
    {
        screen.AddToClassList(className: "screenFadeOut");
        yield return new WaitForSeconds(4);

        //yield return StartCoroutine(AudioController.Instance.PlayEndAudio());
        DialogueController.instance.EndDialogue();
        yield return new WaitForSeconds(2);

        screen.RemoveFromClassList(className: "screenFadeOut");
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(0);

    }

    #region Animations
    public void FadeIn()
    {
        screen.AddToClassList(className: "fade-in");
    }

    public void FadeOut()
    {
        screen.RemoveFromClassList(className: "fade-in");
        screen.AddToClassList(className: "fade-out");
    }
    #endregion

}
