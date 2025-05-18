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
    [SerializeField, Range(0,100)]
    float water = 100.0f;
    [SerializeField, Range(0,100)]
    float hunger = 5.0f;
    [SerializeField, Range(0,100)]
    float plantLight = 100.0f;
    [SerializeField, Range(0, 100)]
    float affection = 100.0f;
    // Accessories - Array that holds accessories
    // Will need to create some obj, 
    // Plant Type
    string plantType = "Pitcher Plant";
    // Plant Stage
    int plantStage = 1;
    // Plant status
    ItemsList.PlantStatus plantStatus = ItemsList.PlantStatus.Sad;
    [SerializeField]
    string plantName = "Twoey";

    public bool petMode = false;



    // Stat Properties
    float waterCurve = 1.25f;
    float affectCurve = 2.0f;


    // State? - In feed mode/Water mode/Light mode - disable controls to only focus on this so those modes can be the same

    // Animation vars
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        petPlant += PetPlant;
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
        if(petMode)
        {
            petMode = false;
        } else
        {
            petMode = true;
        }
    }

    // Feed Plant
    private void FeedPlant(GameObject food)
    {
        int fooditem = (int) ItemsList.FoodItems.Fertilizer;
        // Takes food obj - checks what type of food & extract value
        // depending on food, hunger values will go up
        hunger += fooditem;

        // If food is favourite, 

        Debug.Log("Plant Hunger Level:" + hunger);
    }

    // Water Plant
    private void WaterPlant()
    {
        water += Mathf.Clamp(Mathf.Exp(waterCurve), 0.0f, 2.5f) * Time.deltaTime;

        Debug.Log("Water Increase: " + water);
    }

    // Give plant light

    // Give plant affection
    private void PetPlant()
    {
        affection += Mathf.Clamp(Mathf.Exp(affectCurve), 0.0f, 2.5f) * Time.deltaTime;

        Debug.Log("Plant pet!");
    }

    // Show Plant info
    public string PlantInfo()
    {
        string plantInformation = string.Format("Name: {0}\nHunger:{1}\nWater:{2}\n" +
            "Light:{3}\nAffection:{4}\nStatus:{5}\nPlant Type:{6}\nPlant Stage:{7}\n" +
            "Pet Mode:{8}",
            plantName, hunger, water, plantLight, affection, plantStatus, plantType, plantStage, petMode);

        //Debug.Log(plantInformation);

        return plantInformation;
        
    }

    private void StatusChange()
    {
        // If certain conditions are not met change status
        // Happy - All stats are ~75% =< fulfilled
        // Sad - Affection stat Low
        // Dry - Water Stat in 40% - 60%
        // Wilting - Water Stat in <40%
        // Dying - All stats are below 25%



        if(hunger >= 75.0f && water >= 75.0f && plantLight >= 75.0f && affection >= 75.0f)
        {
            plantStatus = ItemsList.PlantStatus.Happy;
        }

        if(affection <= 40.0f)
        {
            plantStatus = ItemsList.PlantStatus.Sad;
        }

        if(water >= 40.0f && water <= 50.0f)
        {
            plantStatus = ItemsList.PlantStatus.Dry;
        }

        if(water < 40.0f && hunger < 40.0f)
        {
            plantStatus = ItemsList.PlantStatus.Wilting;
        }

        if(hunger < 25.0f && water < 25.0f && plantLight < 25.0f && affection < 25.0f)
        {
            plantStatus = ItemsList.PlantStatus.Dying;
        }

        
    }

    // Method to handle pet,

    /*
     * Check if anything has gone inside plant collision
     * Check item type of item thts gone in collision zone
     * use specific method when item type has been figured out
     * 
     */
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<FoodChoices>().itemType == 1)
        {
            FeedPlant(collision.gameObject);
            // Might need timer to delay to allow animation to play
            Destroy(collision.gameObject);

            // At the end of doing collision things, Trigger ui reset - event?
            Main.resetMainUI?.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<FoodChoices>().itemType == 2)
        {
            WaterPlant();

            Debug.Log("I'm getting watered!");
        }
    }

    #endregion

    #region Animation
    public void PlantAnimation()
    {
        animator.Play("Stage2Happy");
    }
    #endregion
}
