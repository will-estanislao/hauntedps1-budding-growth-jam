using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlantCreature : MonoBehaviour
{
    /**
     * Script to primarily hold plant related behaviour
     * 
     */
    // Events & Delegates
    public delegate void PetAction();
    public static PetAction petPlant;

    // Plant Properties
    [SerializeField, Range(0, 100)]
    private float water;
    public float Water { get => water; }
    [SerializeField, Range(0, 100)]
    private float hunger;
    public float Hunger { get => hunger; }
    [SerializeField, Range(0, 100)]
    private float  affection;
    public float Affection { get => affection; }
    private string plantType = "Pitcher Plant";
    [SerializeField, Range(1,3)]
    private int plantStage;
    public int PlantStage { get => plantStage; }
    private ItemsList.PlantStatus plantStatus;
    public ItemsList.PlantStatus PlantStatus { get => plantStatus; }
    [SerializeField]
    private string plantName;
    public string PlantName { get => plantName; }

    public bool petMode = false;

    private List<ItemsList.FoodItems> favFoods = new List<ItemsList.FoodItems>() { ItemsList.FoodItems.Steak };
    private List<ItemsList.FoodItems> hateFoods = new List<ItemsList.FoodItems>() { ItemsList.FoodItems.MealWorm };

    // Stat Properties
    private float waterCurve = 1.25f;
    private float affectCurve = 2.0f;
    private float lightAdditon = 1.0f;

    ItemsList.LightMode currentMode = ItemsList.LightMode.Medium;
    public ItemsList.LightMode CurrentMode { get => currentMode; }

    // Dialogue Related
    [SerializeField]
    public DialogueAsset plantDialog;
    int startPos;
    bool inConversation;

    // State? - In feed mode/Water mode/Light mode - disable controls to only focus on this so those modes can be the same

    // Animation vars
    private Animator animator;

    // Hashes for animation
    int isHappyHash;
    int isSadHash;
    int isIdleHash;

    

    // This will run on new instantiation....
    private void Awake()
    {
        petPlant += PetPlant;

        animator = GetComponent<Animator>();

        isHappyHash = Animator.StringToHash("Stage2Happy");
        isSadHash = Animator.StringToHash("Stage2Sad");
        isIdleHash = Animator.StringToHash("Stage2Idle");
    }

    public void OnUpdate()
    {
        
        // Do appropriate animation
        // Update plant status
        StatusChange();

    }

    #region Plant Methods
    public void SetPlantName(string newName)
    {
        plantName = newName;
    }

    public void SwitchMode()
    {
        if (petMode)
        {
            petMode = false;
        }
        else
        {
            petMode = true;
        }
    }

    public void SwitchLight(ItemsList.LightMode newMode)
    {
        currentMode = newMode;
        SetLight();
        Main.resetMainUI?.Invoke();
    }

    // Feed Plant
    private void FeedPlant(ItemChoices food)
    {
        float foodCalc = 5.25f;
        // Takes food obj - checks what type of food & extract value
        // depending on food, hunger values will go up
        //Debug.Log(food.foodType.ToString());
        // If food is fav
        if(favFoods.Contains(food.foodType))
        {
            animator.SetTrigger("isHappy");
            foodCalc = (int)food.foodType * 1.25f;
        }
        else if(hateFoods.Contains(food.foodType))
        {
            animator.SetTrigger("isSad");
            foodCalc = (int)food.foodType * 0.75f;
        }

        hunger += foodCalc * lightAdditon;

        hunger = Mathf.Clamp(hunger, 0, 100);

        Debug.Log("Plant Hunger Level:" + hunger);
    }

    // Water Plant
    private void WaterPlant()
    {
        water += Mathf.Clamp(Mathf.Exp(waterCurve), 0.0f, 2.5f) * lightAdditon * Time.deltaTime;
        water = Mathf.Clamp(water, 0, 100);
        Debug.Log("Water Increase: " + water);
    }

    // Give plant light
    private void SetLight()
    {
        // Set the light
        if(currentMode == ItemsList.LightMode.Low)
        {
            lightAdditon = 0.50f;
        }
        else if (currentMode == ItemsList.LightMode.Medium)
        {
            lightAdditon = 1.0f;
        }
        else if(currentMode == ItemsList.LightMode.High)
        {
            lightAdditon = 1.25f;
        }
    }

    // Give plant affection
    private void PetPlant()
    {
        affection += Mathf.Clamp(Mathf.Exp(affectCurve), 0.0f, 2.5f) * Time.deltaTime;
        affection = Mathf.Clamp(affection, 0, 100);

        Debug.Log("Plant pet!");
    }

    // Talk to plant
    public void Talk()
    {
        DialogueController.instance.ShowDialogue(plantDialog.dialogue, startPos, plantName);
    }

    // Show Plant info
    public string PlantInfo()
    {
        string plantInformation = string.Format("Name: {0}\nHunger:{1}\nWater:{2}\n" +
            "Light:{3}\nAffection:{4}\nStatus:{5}\nPlant Type:{6}\nPlant Stage:{7}\n" +
            "Pet Mode:{8}",
            plantName, hunger, water, currentMode, affection, plantStatus, plantType, plantStage, petMode);

        //Debug.Log(plantInformation);

        return plantInformation;

    }

    public bool IsStatZero()
    {
        if (water == 0.0f || hunger == 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlantEndGame()
    {

        //if good end vs bad end
        //if stage3 && endgame Start dialogue line for endgame
        if (inConversation)
        {
            DialogueController.instance.SkipLine();
        }
        else
        {
            if (Main.isGoodEnd)
            {
                DialogueController.instance.ShowDialogue(plantDialog.goodEndDialogue, 0, plantName);
            }
            else
            {
                DialogueController.instance.ShowDialogue(plantDialog.badEndDialogue, 0, plantName);
            }
        }

        // Dialogue

        // Play animation

    }

    private void StatusChange()
    {
        // If certain conditions are not met change status
        // Happy - All stats are ~75% =< fulfilled
        // Sad - Affection stat Low
        // Dry - Water Stat in 40% - 60%
        // Wilting - Water Stat in <40%
        // Dying - All stats are below 25%

        if (hunger >= 65.0f && water >= 65.0f && affection >= 65.0f)
        {
            plantStatus = ItemsList.PlantStatus.Happy;
            startPos = 4;
        }

        if (water > affection && hunger > affection)
        {
            plantStatus = ItemsList.PlantStatus.Sad;
            startPos = 2;
        }

        if (water < affection && water < hunger)
        {
            plantStatus = ItemsList.PlantStatus.Dry;
            startPos = 1;
        }

        if (hunger < water && hunger < affection)
        {
            plantStatus = ItemsList.PlantStatus.Wilting;
            startPos = 0;
        }

        if(hunger >= 40.0f && water >= 40.0f && affection >= 40.0f)
        {
            plantStatus = ItemsList.PlantStatus.Neutral;
        }

        if (hunger <= 25.0f && water <= 25.0f && affection <= 25.0f)
        {
            plantStatus = ItemsList.PlantStatus.Dying;
        }

        // Stat for dead - therefore game over
        // If plant ends stage not happy- leads to bad end

    }

    public void SetPlantStatsOnNewStage(PlantData prevPlantData)
    {
        water = prevPlantData.water;
        hunger = prevPlantData.hunger;
        affection = prevPlantData.affection;
        plantStage = prevPlantData.plantStage;
        plantName = prevPlantData.plantName;
        plantStatus = prevPlantData.plantStatus;
        currentMode = prevPlantData.plantLight;

    }

    private void JoinConvo()
    {
        inConversation = true;
    }

    private void LeaveConvo()
    {
        inConversation = false;
    }

    private void OnEnable()
    {
        DialogueController.OnDialogStarted += JoinConvo;
        DialogueController.OnDialogEnded += LeaveConvo;
    }

    private void OnDisable()
    {
        DialogueController.OnDialogStarted -= JoinConvo;
        DialogueController.OnDialogEnded -= LeaveConvo;
    }
    #endregion

    /*
     * Check if anything has gone inside plant collision
     * Check item type of item thts gone in collision zone
     * use specific method when item type has been figured out
     * 
     */
    private void OnCollisionEnter(Collision collision)
    {
        ItemChoices foodItem = collision.gameObject.GetComponent<ItemChoices>();
        if (foodItem.itemType == 1)
        {
            FeedPlant(foodItem);

            Destroy(collision.gameObject);

            // At the end of doing collision things, Trigger ui reset - event?
            Main.resetMainUI?.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<ItemChoices>().itemType == 2)
        {
            WaterPlant();

            Debug.Log("I'm getting watered!");
        }
    }
}
