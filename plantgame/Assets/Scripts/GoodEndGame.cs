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
        // Dialog box
        DialogueController.instance.DialogBox = dialog;
        DialogueController.instance.DialogueName = DialogueController.instance.DialogBox.Q<Label>(name: "name");
        DialogueController.instance.DialogueText = DialogueController.instance.DialogBox.Q<Label>(name: "dialogText");

        DialogueController.instance.StartDialogue(endDialogue.dialogue, 0, "");
    }

    private void Update()
    {
            OnClick();
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
            if (endDialogue.dialogue.Length - 1 <= dialoguePos)
            {
                
                screen.visible = false;
                StartCoroutine(AudioController.Instance.PlayEndAudio());
                // Then fade
                StartCoroutine(EndGame());
            }
            else
            {
                DialogueNext();
            }
            
        }
    }

    public IEnumerator EndGame()
    {
        StopCoroutine(AudioController.Instance.PlayEndAudio());
        screen.AddToClassList("fade-in");
        yield return new WaitForSeconds(15);


        //SceneManager.LoadScene(0);

    }
   
}
