using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    // When one of the buttons on the ui is clicked, 
    // ex feed
    // Make meat show up


    private UIDocument _document;

    private Button _button;

    // Subscribing multiple buttons to events
    private List<Button> _careMenuButtons = new List<Button>();
    private VisualElement foodMenu;
    private VisualElement debugMenu;
    private VisualElement mainMenu;
    private Button meatButton;
    private Button petBtn;
    private Button exitButton;

    [SerializeField]
    public Texture2D mouseCursor;

    // UI
    Vector3 mousePos;
    Vector3 objPos;
    Canvas uiCanvas;

    [SerializeField]
    public GameObject meat;

    private GameObject currentObject;

    private void Awake()
    {

        _document = GetComponent<UIDocument>();

        exitButton = _document.rootVisualElement.Q<Button>(name: "exitBtn");

        foodMenu = _document.rootVisualElement.Q(name: "FoodMenu");
        mainMenu = _document.rootVisualElement.Q(name: "main");
        debugMenu = _document.rootVisualElement.Q(name: "DebugUI");
       
        meatButton = foodMenu.Q<Button>("meatOption");
        meatButton.RegisterCallback<ClickEvent>(SpawnFood);

        petBtn = mainMenu.Q<Button>("petButton");

        // Hide foodmenu until feed is clicked
        foodMenu.visible = false;
        exitButton.visible = false;

        // Get the heirarchy of the ui element within ex: trying to get button
        _button = _document.rootVisualElement.Q("feedButton") as Button;

        _button.RegisterCallback<ClickEvent>(OnFeedClick);

        exitButton.RegisterCallback<ClickEvent>(OnExit);
        exitButton.RegisterCallback<ClickEvent>(OnAllButtonsClicked);

        petBtn.RegisterCallback<ClickEvent>(OnPet);

        _careMenuButtons = _document.rootVisualElement.Query<Button>(className:"care-menu-btn").ToList();
        for(int i = 0; i < _careMenuButtons.Count; i++)
        {
            _careMenuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClicked);
            Debug.Log(_careMenuButtons[i].name);
        }

        _careMenuButtons[1].RegisterCallback<ClickEvent>(OnWatering);

    }

    public void OnUIUpdate(string info)
    {
        // Update Debug Menu
        debugMenu.Q<Label>(name: "plantStats").text = "Plant Stats:\n" + info;


    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnFeedClick);

        exitButton.UnregisterCallback<ClickEvent>(OnExit);
        petBtn.UnregisterCallback<ClickEvent>(OnPet);

        for (int i = 0; i < _careMenuButtons.Count; i++)
        {
            _careMenuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClicked);
        }
    }

    private void OnExit(ClickEvent evnt)
    {
        Debug.Log("Exit Button was pressed");
        // Despawn Object
        Main.despawnObject?.Invoke();
        
        if(Main.plantMode)
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

    public void SpawnFood(ClickEvent evnt)
    {
        Debug.Log("Target:" + evnt.currentTarget);

        Main.spawnObject("Meat");

        foodMenu.visible = false;

    }

    private void OnWatering(ClickEvent evnt)
    {

        _document.rootVisualElement.Q(name: "main").visible = false;
        exitButton.visible = true;

        // Spawn item in
        Main.spawnObject("Water");

    }

    private void OnPet(ClickEvent evnt)
    {
        _document.rootVisualElement.Q(name: "main").visible = false;
        exitButton.visible = true;

        UnityEngine.Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Main.switchState?.Invoke();
        //Main.createCursor?.Invoke();
    }

    // Assign everything here that applies to all buttons
    private void OnAllButtonsClicked(ClickEvent click)
    {
        Debug.Log("Button Click was successful\n Button Assignment was a success");
    }

    public void ResetUI()
    {
        _document.rootVisualElement.Q(name: "main").visible = true;

        _document.rootVisualElement.Q(name: "FoodMenu").visible = false;
    }

    private void DebugMenuUpdate()
    {
        //string plantStatus = "Plant Stats:\n Name: " + ;

        //debugMenu.Q<Label>(name: "plantStats").text = plantStatus;
    }
}
