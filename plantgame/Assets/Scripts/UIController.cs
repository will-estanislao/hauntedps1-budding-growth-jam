using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    // When one of the buttons on the ui is clicked, 
    // ex feed
    // Make meat show up


    private UIDocument _document;

    private Button _button;

    // Subscribing multiple buttons to events
    private List<Button> _careMenuButtons = new List<Button>();
    private List<Button> allButtons = new List<Button>();
    private VisualElement subMenu;
    private VisualElement debugMenu;
    private VisualElement mainMenu;
    private VisualElement nextDay;

    private VisualElement foodMenu;
    private VisualElement lightMenu;

    private Label infoTxt;
    private Label title;

    private Button meatButton;
    private Button wormBtn;
    private Button eggBtn;
    private Button fertilizerBtn;
    private Button flyBtn;
    private Button exitButton;
    private Button lowLightBtn;
    private Button medLightBtn;
    private Button highLightBtn;

    [SerializeField]
    public Texture2D mouseCursor;

    [SerializeField]
    public GameObject meat;

    private GameObject currentObject;

    private void Awake()
    {
        // Get ui elements on screen
        _document = GetComponent<UIDocument>();
        exitButton = _document.rootVisualElement.Q<Button>(name: "exitBtn");
        subMenu = _document.rootVisualElement.Q(name: "SubMenu");
        mainMenu = _document.rootVisualElement.Q(name: "main");
        debugMenu = _document.rootVisualElement.Q(name: "DebugUI");
        nextDay = _document.rootVisualElement.Q(name: "nextDay");
        foodMenu = subMenu.Q<VisualElement>(name: "FoodMenu");
        lightMenu = subMenu.Q<VisualElement>(name: "LightMenu");
        title = nextDay.Q<Label>(name: "titleCard");

        meatButton = foodMenu.Q<Button>("Steak");
        wormBtn = foodMenu.Q<Button>("Mealworm");
        flyBtn = foodMenu.Q<Button>("Fly");
        fertilizerBtn = foodMenu.Q<Button>("Fertilizer");
        eggBtn = foodMenu.Q<Button>("Eggshell");
        // Assign events to food buttons

        meatButton.clickable.clickedWithEventInfo += Spawn;
        wormBtn.clickable.clickedWithEventInfo += Spawn;
        flyBtn.clickable.clickedWithEventInfo += Spawn;
        eggBtn.clickable.clickedWithEventInfo += Spawn;
        fertilizerBtn.clickable.clickedWithEventInfo += Spawn;


        lowLightBtn = lightMenu.Q<Button>("Low");
        medLightBtn = lightMenu.Q<Button>("Med");
        highLightBtn = lightMenu.Q<Button>("High");

        exitButton.clickable.clicked += OnExit;

        _careMenuButtons = mainMenu.Query<Button>(className: "care-menu-btn").ToList();
        _careMenuButtons[0].clickable.clicked += OnFeedClick;
        _careMenuButtons[1].clickable.clicked += OnWatering;
        _careMenuButtons[2].clickable.clicked += OnPet;
        _careMenuButtons[3].clickable.clicked += OnLight;
        _careMenuButtons[4].clickable.clicked += OnTalk;
        _careMenuButtons[5].clickable.clicked += OnNextDay;

        lowLightBtn.clickable.clickedWithEventInfo += SetLight;
        medLightBtn.clickable.clickedWithEventInfo += SetLight;
        highLightBtn.clickable.clickedWithEventInfo += SetLight;

        allButtons = _document.rootVisualElement.Query<Button>(className: "unity-button").ToList();
        foreach (Button btn in allButtons)
        {
            btn.clickable.clicked += OnAllButtonsClicked;
        }

        infoTxt = mainMenu.Q<Label>(name: "info");
    }

    public void Start()
    {
        // Hide specific ui elements
        subMenu.visible = false;
        exitButton.visible = false;

        DisableNextDay();

        // Dialog box
        DialogueController.instance.DialogBox = _document.rootVisualElement.Q(name: "dialogBox");
        DialogueController.instance.DialogueName = DialogueController.instance.DialogBox.Q<Label>(name: "name");
        DialogueController.instance.DialogueText = DialogueController.instance.DialogBox.Q<Label>(name: "dialogText");

        DialogueController.instance.DialogBox.visible = false;

        Debug.Log(mainMenu);
    }

    private void OnDisable()
    {
        _careMenuButtons[0].clickable.clicked -= OnFeedClick;
        _careMenuButtons[1].clickable.clicked -= OnWatering;
        _careMenuButtons[2].clickable.clicked -= OnPet;
        _careMenuButtons[3].clickable.clicked -= OnLight;
        _careMenuButtons[4].clickable.clicked -= OnTalk;
        _careMenuButtons[5].clickable.clicked -= OnNextDay;
        
        foreach (Button btn in allButtons)
        {
            btn.clickable.clicked -= OnAllButtonsClicked;
        }
        

        lowLightBtn.clickable.clickedWithEventInfo -= SetLight;
        medLightBtn.clickable.clickedWithEventInfo -= SetLight;
        highLightBtn.clickable.clickedWithEventInfo -= SetLight;

        meatButton.clickable.clickedWithEventInfo -= Spawn;
        wormBtn.clickable.clickedWithEventInfo -= Spawn;
        flyBtn.clickable.clickedWithEventInfo -= Spawn;
        eggBtn.clickable.clickedWithEventInfo -= Spawn;
        fertilizerBtn.clickable.clickedWithEventInfo -= Spawn;

        exitButton.clickable.clicked -= OnExit;
    }


    public void OnUIUpdate(string info)
    {
        // Update Debug Menu
        DebugMenuUpdate(info);
        infoTxt.text = "Day: " + Main.currentDay + " Stage: " + Main.Instance.plantDataSave.plantStage + " Light Mode: " + Main.Instance.plantDataSave.plantLight;

    }

    private void OnNextDay( )
    {
        mainMenu.SetEnabled(false);
        nextDay.visible = true;
        if(Main.Instance.plantDataSave.plantStage == 3)
        {
            Invoke(nameof(OnEndGame), 2);
        }
        else
        {
            Main.Instance.Invoke("SetUpNewStage", 2);
        }
    }

    private void OnEndGame()
    {
        mainMenu.visible = false;
        nextDay.visible = false;
        StartCoroutine(Main.Instance.EndGame());
    }

    private void OnExit()
    {
        Debug.Log("Exit Button was pressed");
        // Despawn Object
        Main.despawnObject?.Invoke();

        if (Main.plantMode)
        {
            Main.switchState?.Invoke();
        }

        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        exitButton.visible = false;

        AudioController.Instance.StopPlay();
        ResetUI();
    }

    // Dont use event, its keyword
    private void OnFeedClick()
    {
        Debug.Log("Food Clicked!");
        mainMenu.SetEnabled(false);
        foodMenu.SetEnabled(true);
        exitButton.SetEnabled(true);
        foodMenu.visible = true;
        exitButton.visible = true;
        mainMenu.visible = false;

    }

    public void Spawn(EventBase evnt)
    {
        Button button = (Button)evnt.target;
        ItemsList.FoodItems food = ItemsList.FoodItems.Steak;

        if (button.name == "Steak")
        {
            food = ItemsList.FoodItems.Steak;
        }
        else if (button.name == "Mealworm")
        {
            food = ItemsList.FoodItems.MealWorm;
        }
        else if (button.name == "Fertilizer")
        {
            food = ItemsList.FoodItems.Fertilizer;
        }
        else if (button.name == "Eggshell")
        {
            food = ItemsList.FoodItems.EggShell;
        }
        else if (button.name == "Fly")
        {
            food = ItemsList.FoodItems.Fly;
        }

        Main.spawnObject?.Invoke(1, food);

        foodMenu.visible = false;
        foodMenu.SetEnabled(false);
        // components we need- which button was pressed
        // to determine which food to spawn

    }

    private void OnWatering()
    {
        mainMenu.SetEnabled(false);
        exitButton.SetEnabled(true);
        mainMenu.visible = false;
        exitButton.visible = true;

        // Spawn item in
        Main.spawnObject(2, ItemsList.FoodItems.None);
    }

    private void OnLight()
    {
        // Bring up a light settings menu
        // Low, Mid, High
        // These just affect stat- specifically water and food increase/decrease
        mainMenu.SetEnabled(false);
        lightMenu.SetEnabled(true);
        exitButton.SetEnabled(true);
        mainMenu.visible = false;
        exitButton.visible = true;
        lightMenu.visible = true;
    }

    private void SetLight(EventBase evnt)
    {
        Button button = (Button)evnt.target;
        ItemsList.LightMode lightChoice = ItemsList.LightMode.Medium;

        if (button.name == "Low")
        {
            lightChoice = ItemsList.LightMode.Low;
        }
        else if (button.name == "Med")
        {
            lightChoice = ItemsList.LightMode.Medium;
        }
        else if (button.name == "High")
        {
            lightChoice = ItemsList.LightMode.High;
        }

        Main.lightState?.Invoke(lightChoice);

        lightMenu.visible = false;
        lightMenu.SetEnabled(false);
    }

    private void OnPet()
    {
        mainMenu.SetEnabled(false);
        exitButton.SetEnabled(true);
        mainMenu.visible = false;
        exitButton.visible = true;

        UnityEngine.Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Main.switchState?.Invoke();
    }

    private void OnTalk( )
    {
        mainMenu.SetEnabled(false);
        mainMenu.visible = false;
        Main.Instance.PlantTalk();
    }

    // Assign everything here that applies to all buttons
    private void OnAllButtonsClicked()
    {
        Debug.Log("Button Click was successful\n Button Assignment was a success");
        AudioController.Instance.PlayClick();
    }

    public void EnableNextDay()
    {
        _careMenuButtons[5].SetEnabled(true);
    }

    public void DisableNextDay()
    {
        _careMenuButtons[5].SetEnabled(false);
    }

    public void ResetUI()
    {
        mainMenu.visible = true;
        exitButton.visible = false;
        nextDay.visible = false;
        foodMenu.visible = false;
        lightMenu.visible = false;
        subMenu.visible = false;
        DialogueController.instance.DialogBox.visible = false;

        mainMenu.SetEnabled(true);
        exitButton.SetEnabled(false);
        
    }

    public void HideAllUI()
    {
        mainMenu.visible = false;
        exitButton.visible = false;
        nextDay.visible = false;
        subMenu.visible = false;
    }

    public void HideScreen()
    {
        title.visible = false;
        nextDay.visible = true;
    }

    private void DebugMenuUpdate(string info)
    {
        debugMenu.Q<Label>(name: "plantStats").text = "Plant Stats:\n" + info;
    }

}
