using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{
    // Events & Delegates
    public delegate void OnUIReset();
    public static OnUIReset resetMainUI;
    
    public delegate void ChangePlantState();
    public static ChangePlantState switchState;

    public delegate void OnDespawn();
    public static OnDespawn despawnObject;

    public delegate void ObjectSpawn(string name, int itemType);
    public static ObjectSpawn spawnObject;

    /*
    public delegate void SpawnCursor();
    public static SpawnCursor createCursor;
    */

    // Stats
    private int day;


    // UI
    Vector3 mousePos;
    Vector3 objPos;
    Canvas uiCanvas;

    // Mouse Controls
    Ray ray;
    RaycastHit hit;

    // Plant Game OBJ
    private GameObject currentPlant;


    private GameObject currentItem;
    public GameObject[] allItems;

    [SerializeField]
    public Camera currentCam;

    [SerializeField]
    public UIController gameUI;

    //[SerializeField]
    //public GameObject hand;
    //private GameObject petHand;

    public static bool plantMode;
    private Ray rayCast;
    private RaycastHit hitData;

    // Mouse Cursor
    [SerializeField]
    public Texture2D cursorTexture;

    private void Awake()
    {

        currentPlant = GameObject.FindWithTag("Player");
        gameUI = GameObject.Find("UI").GetComponent<UIController>();
        plantMode = currentPlant.GetComponent<PlantCreature>().petMode;

        resetMainUI += ResetUI;
        resetMainUI += UIUpdates;

        spawnObject += SpawnObject;
        
        despawnObject += DespawnObject;

        switchState += ChangeMode;

        //createCursor += InstantiateHand;

    }

    // Start is called before the first frame update
    void Start()
    {
        UIUpdates();    // Show current plant stats
        print(currentPlant);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for food if there is there
        currentItem = GameObject.FindWithTag("Item");

        mousePos = currentCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        rayCast = currentCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && currentItem != null)
        {
            // Turn on collision for plant

            currentItem.GetComponent<FoodChoices>().OnUpdate();

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
            }
        }

        //DebugLogs();
        currentPlant.GetComponent<PlantCreature>().OnUpdate();
        // Once plant has updated, bring back all UI things
        UIUpdates();

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

    public void ResetUI()
    {
        gameUI.ResetUI();
    }

    /*
    public void InstantiateHand()
    {
        if(!hand.activeInHierarchy)
        {
            Debug.Log("Hand Instantiated");
            petHand = Instantiate(hand, new Vector3(mousePos.x, mousePos.y, 0.0f), hand.transform.rotation);
        }
    }
    */

    public void UIUpdates()
    {
        gameUI.OnUIUpdate(currentPlant.GetComponent<PlantCreature>().PlantInfo());
    }

    private void SpawnObject(string name, int itemType)
    {
        GameObject objToSpawn = LoadPrefabFromFile(name);
        Vector3 spawnPos = new Vector3(4, 3, 0);
        Instantiate(objToSpawn, spawnPos, objToSpawn.transform.rotation);

        // If

    }

    private void DespawnObject()
    {
        //Debug.Log("Object in hierarchy: " + currentItem.activeInHierarchy.ToString());
        if(currentItem != null)
        {
            Destroy(currentItem);
        }
        /*
        //Debug.Log("Hand in hierarchy: " + hand.activeInHierarchy);
        if (petHand != null)
        {
            Destroy(petHand);
        }
        */
    }

    #region Loading Asset
    private UnityEngine.GameObject LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load LevelPrefab from file (" + filename + ")...");
        GameObject loadedObject = (GameObject)Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }
    #endregion



}
