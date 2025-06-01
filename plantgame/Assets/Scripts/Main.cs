using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class Main : MonoBehaviour
{
    // Events & Delegates
    public delegate void OnUIReset();
    public static OnUIReset resetMainUI;

    public delegate void ChangePlantState();
    public static ChangePlantState switchState;

    public delegate void ChangeLightState(ItemsList.LightMode light);
    public static ChangeLightState lightState;

    public delegate void OnDespawn();
    public static OnDespawn despawnObject;

    public delegate void ObjectSpawn(int itemType, ItemsList.FoodItems foodType);
    public static ObjectSpawn spawnObject;

    private float CAMDISTANCE = 15.0f;

    // Stats
    public int currentStage;
    public static int currentDay;

    // Plant Game OBJ
    private GameObject currentPlant;
    private GameObject currentItem;
    private string fileName;
    private Vector3 plantSpot = new Vector3(0, 1, 0);

    [SerializeField]
    private GameObject Stage1Plant;
    [SerializeField]
    private GameObject Stage2Plant;
    [SerializeField]
    private GameObject Stage3Plant;
    GameObject tempObj;

    [SerializeField]
    public Camera currentCam;

    [SerializeField]
    public UIController gameUI;

    public static bool gameEnd;
    public static bool isGoodEnd;
    public static bool plantMode;
    public static bool inConvo;

    Vector3 mousePos;
    private Ray rayCast;
    private RaycastHit hitData;

    // Mouse Cursor
    [SerializeField]
    public Texture2D cursorTexture;

    [SerializeField]
    public PlantData plantDataSave;

    public List<ItemsList.PlantStatus> endStatus;

    public static Main Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        resetMainUI += ResetUI;
        resetMainUI += UIUpdates;

        spawnObject += SpawnObject;
        
        despawnObject += DespawnObject;

        switchState += ChangeMode;
        lightState += ChangeLight;

        inConvo = false;
        gameEnd = false;
        isGoodEnd = false;
        currentPlant = Instantiate(Stage1Plant);
        tempObj = new GameObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        gameUI = GameObject.Find("UI").GetComponent<UIController>();
        plantMode = currentPlant.GetComponent<PlantCreature>().petMode;

        plantDataSave.UpdateCurrentStats(currentPlant.GetComponent<PlantCreature>());

        UIUpdates();    // Show current plant stats
        currentStage = plantDataSave.plantStage;
        currentDay = 1;
        print(currentPlant);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for food if there is there
        currentItem = GameObject.FindWithTag("Item");

        mousePos = currentCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CAMDISTANCE));
        rayCast = currentCam.ScreenPointToRay(Input.mousePosition);

        if(!gameEnd)
        {
            if (Input.GetMouseButton(0) && currentItem != null)
            {
                // Turn on collision for plant

                currentItem.GetComponent<ItemChoices>().OnUpdate();

            }

            if (Input.GetMouseButton(0) && plantMode)
            {
                // Play hand animation
                //petHand.GetComponent<Animation>().Play();
                // Create a ray from mouse pos, if mouse pos hits specifically something
                // and it is plant obj, call on pet
                if (Physics.Raycast(rayCast, out hitData) && hitData.transform.gameObject.CompareTag("Player"))
                {
                    PlantCreature.petPlant?.Invoke();
                    StartCoroutine(AudioController.Instance.PlayPetSound());
                }
            }

            //DebugLogs();
            if (currentPlant != null)
            {
                currentPlant.GetComponent<PlantCreature>().OnUpdate();
                plantDataSave.UpdateCurrentStats(currentPlant.GetComponent<PlantCreature>());
                UIUpdates();
            }
        }

        OnClick();
        // Check if its the last stage and start end game

    }

    public void DebugLogs()
    {
        Debug.Log("Mouse Position - Camera: " + mousePos);
        Debug.Log("Mouse Position - Input raw: " + Input.mousePosition);
        Debug.Log("Current Item Out: " + currentItem.transform.position);
    }

    public void ChangeMode()
    {
        currentPlant.GetComponent<PlantCreature>().SwitchMode();
        plantMode = currentPlant.GetComponent<PlantCreature>().petMode;
    }

    public void ChangeLight(ItemsList.LightMode newMode)
    {
        currentPlant.GetComponent<PlantCreature>().SwitchLight(newMode);
    }

    public void ResetUI()
    {
        gameUI.ResetUI();
    }

    public void PlantTalk()
    {
        currentPlant.GetComponent<PlantCreature>().Talk();
    }

    public void UIUpdates()
    {
        gameUI.OnUIUpdate(currentPlant.GetComponent<PlantCreature>().PlantInfo());
    }

    public void InConversation()
    {
        inConvo = true;
        
    }

    public void EndConversation()
    {
        inConvo = false;
    }

    public void SetUpNewStage()
    {
        
        // Save the current plants data
        plantDataSave.UpdateCurrentStats(currentPlant.GetComponent<PlantCreature>());
        endStatus.Add(plantDataSave.plantStatus);

        // Destroy plant obj
        if(currentPlant != null)
        {
            Destroy(currentPlant);
        }
        
        if (plantDataSave.plantStage == 1)
        {
            tempObj = LoadPrefabFromFile("Stage2PlantPrefab");
            
        }
        else if (plantDataSave.plantStage == 2)
        {
            // Move camera a bit
            tempObj = LoadPrefabFromFile("Stage3Plant");
            
        }

        plantDataSave.SetStageStats();

        currentPlant = Instantiate(tempObj);

        // Set prev plant data to new plant
        currentPlant.GetComponent<PlantCreature>().SetPlantStatsOnNewStage(plantDataSave);

        currentDay++;
        currentStage = plantDataSave.plantStage;

        ResetUI();

    }

    public IEnumerator EndGame()
    {
        plantDataSave.UpdateCurrentStats(currentPlant.GetComponent<PlantCreature>());
        endStatus.Add(plantDataSave.plantStatus);
        gameEnd = true;

        int endGameCount = 0;
        // Check on whether its good end
        foreach(ItemsList.PlantStatus status in endStatus)
        {
            if(status == ItemsList.PlantStatus.Happy)
            {
                endGameCount++;
            }
        }

        if(endGameCount == 3)
        {
            isGoodEnd = true;
        }

        currentPlant.GetComponent<PlantCreature>().PlantEndGame();
        yield return new WaitUntil(() => !inConvo);

        // load new screen
        Debug.Log("GameEnd");
        if(!isGoodEnd)
        {
            // Animation play - make sure it ends before
            currentPlant.GetComponent<PlantCreature>().animator.SetTrigger("isEnd");
            yield return new WaitForSeconds(5);
            GameOver();
            //Invoke(nameof(GameOver), 2);
        }
        else
        {
            // Go to good end screen
        }
    }

    public void OnClick()
    {
        if(Input.GetMouseButtonDown(0) && inConvo && gameEnd)
        {
            DialogueController.instance.DisplayNextSentence();
        }
        else if(Input.GetMouseButtonDown(0) && inConvo && !gameEnd)
        {
            ResetUI();
        }
    }

    public void GameOver()
    {
        gameUI.HideAllUI();

        StopAllCoroutines();
        
        // game over screen
        SceneManager.LoadScene(2);
    }

    private void SpawnObject(int itemType, ItemsList.FoodItems foodType)
    {

        GameObject objToSpawn = LoadPrefabFromFile("Item");

        objToSpawn.GetComponent<ItemChoices>().itemType = itemType;
        objToSpawn.GetComponent<ItemChoices>().foodType = foodType;

        Vector3 spawnPos = new Vector3(4, 3, 0);
        Instantiate(objToSpawn, spawnPos, objToSpawn.transform.rotation);

    }

    private void DespawnObject()
    {
        //Debug.Log("Object in hierarchy: " + currentItem.activeInHierarchy.ToString());
        if(currentItem != null)
        {
            Destroy(currentItem);
        }
    }

    #region Loading Asset
    private UnityEngine.GameObject LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load Prefab from file (" + filename + ")...");
        GameObject loadedObject = (GameObject)Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }
    #endregion



}
