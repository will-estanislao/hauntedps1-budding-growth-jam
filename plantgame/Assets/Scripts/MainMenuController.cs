using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument doc;
    private List<Button> mainMenuButtons = new List<Button>();
    private VisualElement mainMenu;
    private Label gameTitle;

    private Button exit;

    private VisualElement credits;


    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        mainMenu = doc.rootVisualElement.Q(name: "mainMenu");
        gameTitle = mainMenu.Q<Label>(name: "title");
        credits = doc.rootVisualElement.Q<VisualElement>(name: "creditsPanel");
        exit = credits.Q<Button>(name: "exit");

        mainMenuButtons = mainMenu.Query<Button>(className: "menu-btn").ToList();
        

    }

    private void Start()
    {
        mainMenuButtons[0].RegisterCallback<ClickEvent>(LoadGame);
        mainMenuButtons[1].RegisterCallback<ClickEvent>(QuitGame);
        mainMenuButtons[2].RegisterCallback<ClickEvent>(ShowCredits);
        exit.RegisterCallback<ClickEvent>(CloseCredits);
        credits.visible = false;

        
    }

    private void OnDisable()
    {
        mainMenuButtons[0].UnregisterCallback<ClickEvent>(LoadGame);
        mainMenuButtons[1].UnregisterCallback<ClickEvent>(QuitGame);
        mainMenuButtons[2].UnregisterCallback<ClickEvent>(ShowCredits);
        exit.UnregisterCallback<ClickEvent>(CloseCredits);
    }

    public void LoadGame(ClickEvent evnt)
    {
        // Load Game Scene
        SceneManager.LoadScene(1);
    }

    public void QuitGame(ClickEvent evnt)
    {
        Application.Quit();
    }

    public void ShowCredits(ClickEvent evnt)
    {
        mainMenu.visible = false;
        credits.visible = true;
        
    }

    public void CloseCredits(ClickEvent evnt)
    {
        
        credits.visible = false;
        mainMenu.visible = true;

    }
}
