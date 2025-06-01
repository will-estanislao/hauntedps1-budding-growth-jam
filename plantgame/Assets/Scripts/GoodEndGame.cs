using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoodEndGame : MonoBehaviour
{
    [SerializeField]
    DialogueAsset endDialogue;

    // UI
    private UIDocument doc;
    private VisualElement screen;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        screen = doc.rootVisualElement.Q(name: "screen");
    }

    private void Start()
    {
        // Dialog box
        DialogueController.instance.DialogBox = doc.rootVisualElement.Q(name: "dialogBox");
        DialogueController.instance.DialogueName = DialogueController.instance.DialogBox.Q<Label>(name: "name");
        DialogueController.instance.DialogueText = DialogueController.instance.DialogBox.Q<Label>(name: "dialogText");

        DialogueController.instance.DialogBox.visible = false;

        DialogueController.instance.StartDialogue(endDialogue.dialogue, "...");
    }

    private void Update()
    {
        OnClick();
    }

    public void DialogueNext()
    {
       
    }

    public void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DialogueController.instance.DisplayNextSentence();
        }
    }
}
