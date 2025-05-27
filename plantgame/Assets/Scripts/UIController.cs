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
    private VisualElement subMenu;
    private VisualElement debugMenu;
    private VisualElement mainMenu;
    private VisualElement nextDay;

    private VisualElement foodMenu;
    private VisualElement lightMenu;

    private Button meatButton;
    private Button wormBtn;
    private Button petBtn;
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

        _document = GetComponent<UIDocument>();
        exitButton = _document.rootVisualElement.Q<Button>(name: "exitBtn");

        subMenu = _document.rootVisualElement.Q(name: "SubMenu");
        mainMenu = _document.rootVisualElement.Q(name: "main");
        debugMenu = _document.rootVisualElement.Q(name: "DebugUI");
        nextDay = _document.rootVisualElement.Q(name: "nextDay");

        foodMenu = subMenu.Q<VisualElement>(name: "FoodMenu");
        lightMenu = subMenu.Q<VisualElement>(name: "LightMenu");

        meatButton = foodMenu.Q<Button>("Steak");
        wormBtn = foodMenu.Q<Button>("Mealworm");
        meatButton.RegisterCallback<ClickEvent, ItemsList.FoodItems>(SpawnFood, ItemsList.FoodItems.Steak);
        wormBtn.RegisterCallback<ClickEvent, ItemsList.FoodItems>(SpawnFood, ItemsList.FoodItems.MealWorm);

        petBtn = mainMenu.Q<Button>("petButton");

        lowLightBtn = lightMenu.Q<Button>("Low");
        medLightBtn = lightMenu.Q<Button>("Med");
        highLightBtn = lightMenu.Q<Button>("High");

        // Hide foodmenu until feed is clicked
        subMenu.visible = false;
        exitButton.visible = false;

        // Get the heirarchy of the ui element within ex: trying to get button
        _button = _document.rootVisualElement.Q("feedButton") as Button;

        _button.RegisterCallback<ClickEvent>(OnFeedClick);

        exitButton.RegisterCallback<ClickEvent>(OnExit);
        exitButton.RegisterCallback<ClickEvent>(OnAllButtonsClicked);

        petBtn.RegisterCallback<ClickEvent>(OnPet);

        _careMenuButtons = _document.rootVisualElement.Query<Button>(className: "care-menu-btn").ToList();
        for (int i = 0; i < _careMenuButtons.Count; i++)
        {
            _careMenuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClicked);
            Debug.Log(_careMenuButtons[i].name);
        }

        _careMenuButtons[1].RegisterCallback<ClickEvent>(OnWatering);
        _careMenuButtons[3].RegisterCallback<ClickEvent>(OnLight);
        _careMenuButtons[4].RegisterCallback<ClickEvent>(OnTalk);
        _careMenuButtons[5].RegisterCallback<ClickEvent>(OnNextDay);

        lowLightBtn.RegisterCallback<ClickEvent, ItemsList.LightMode>(SetLight, ItemsList.LightMode.Low);
        medLightBtn.RegisterCallback<ClickEvent, ItemsList.LightMode>(SetLight, ItemsList.LightMode.Medium);
        highLightBtn.RegisterCallback<ClickEvent, ItemsList.LightMode>(SetLight, ItemsList.LightMode.High);



    }

    public void Start()
    {
        // Dialog box
        DialogueController.instance.DialogBox = _document.rootVisualElement.Q(name: "dialogBox");
        DialogueController.instance.DialogueName = DialogueController.instance.DialogBox.Q<Label>(name: "name");
        DialogueController.instance.DialogueText = DialogueController.instance.DialogBox.Q<Label>(name: "dialogText");

        DialogueController.instance.DialogBox.visible = false;
    }

    public void OnUIUpdate(string info)
    {
        // Update Debug Menu
        debugMenu.Q<Label>(name: "plantStats").text = "Plant Stats:\n" + info;

    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnFeedClick);
        //meatButton.UnregisterCallback<ClickEvent, ItemsList>(SpawnFood);
        //wormBtn.UnregisterCallback<ClickEvent>(SpawnFood);

        exitButton.UnregisterCallback<ClickEvent>(OnExit);
        petBtn.UnregisterCallback<ClickEvent>(OnPet);

        for (int i = 0; i < _careMenuButtons.Count; i++)
        {
            _careMenuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClicked);
        }

        _careMenuButtons[3].UnregisterCallback<ClickEvent>(OnLight);

        lowLightBtn.UnregisterCallback<ClickEvent, ItemsList.LightMode>(SetLight);
        medLightBtn.UnregisterCallback<ClickEvent, ItemsList.LightMode>(SetLight);
        highLightBtn.UnregisterCallback<ClickEvent, ItemsList.LightMode>(SetLight);
    }

    private void OnNextDay(ClickEvent evnt)
    {
        nextDay.visible = true;
        //Main.Instance.SetUpNewStage();
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
        Main.gameEnd = true;
        Main.Instance.EndGame();
    }

    private void OnExit(ClickEvent evnt)
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

        ResetUI();
    }

    // Dont use event, its keyword
    private void OnFeedClick(ClickEvent evnt)
    {
        Debug.Log("Food Clicked!");

        foodMenu.visible = true;
        exitButton.visible = true;

        _document.rootVisualElement.Q(name: "main").visible = false;

    }

    public void SpawnFood(ClickEvent evnt, ItemsList.FoodItems food)
    {
        Debug.Log("Target:" + evnt.currentTarget);

        Main.spawnObject?.Invoke(1, food);

        foodMenu.visible = false;

    }

    private void OnWatering(ClickEvent evnt)
    {

        _document.rootVisualElement.Q(name: "main").visible = false;
        exitButton.visible = true;

        // Spawn item in
        Main.spawnObject(2, ItemsList.FoodItems.None);

    }

    private void OnLight(ClickEvent evnt)
    {
        // Bring up a light settings menu
        // Low, Mid, High
        // These just affect stat- specifically water and food increase/decrease
        _document.rootVisualElement.Q(name: "main").visible = false;
        exitButton.visible = true;
        lightMenu.visible = true;
    }

    private void SetLight(ClickEvent evnt, ItemsList.LightMode lightChoice)
    {
        Main.lightState?.Invoke(lightChoice);

        lightMenu.visible = false;
    }

    private void OnPet(ClickEvent evnt)
    {
        _document.rootVisualElement.Q(name: "main").visible = false;
        exitButton.visible = true;

        UnityEngine.Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Main.switchState?.Invoke();
        //Main.createCursor?.Invoke();
    }

    private void OnTalk(ClickEvent evnt)
    {
        _document.rootVisualElement.Q(name: "main").visible = false;
        Main.talkPlant?.Invoke();
        DialogueController.instance.DialogBox.visible = true;

        // Set this as event tht fires along with interact of the plant
        // in plant decide what type of dialogue to load up
        // here us just show dialogue
        // Player interact event
        
    }

    // Assign everything here that applies to all buttons
    private void OnAllButtonsClicked(ClickEvent click)
    {
        Debug.Log("Button Click was successful\n Button Assignment was a success");
    }

    public void ResetUI()
    {
        _document.rootVisualElement.Q(name: "main").visible = true;
        exitButton.visible = false;
        nextDay.visible = false;

        _document.rootVisualElement.Q(name: "FoodMenu").visible = false;
        DialogueController.instance.DialogBox.visible = false;
    }

    private void DebugMenuUpdate()
    {
        //string plantStatus = "Plant Stats:\n Name: " + ;

        //debugMenu.Q<Label>(name: "plantStats").text = plantStatus;
    }
}
