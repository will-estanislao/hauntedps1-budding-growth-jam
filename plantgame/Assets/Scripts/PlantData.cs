using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    public float water;
    public float hunger;
    public float affection;
    public int plantStage;
    public string plantName;
    public ItemsList.PlantStatus plantStatus;
    public ItemsList.LightMode plantLight;
    public Transform plantLocation;

    private float MINSTAT = 10.0f;
    private float MAXSTAT = 100.0f;


    // Stage start methods
    public void SetStageStats()
    {
        // If plant status is a certain way, reduce initial stat 
        // Make sure to never go lower than 10
        if(plantStatus == ItemsList.PlantStatus.Happy)
        {
            water = Mathf.Clamp((water * 0.65f) - 2.0f, MINSTAT, MAXSTAT);
            hunger = Mathf.Clamp((hunger * 0.65f) - 2.0f, MINSTAT, MAXSTAT);
            affection = Mathf.Clamp((affection * 0.65f) - 2.0f, MINSTAT, MAXSTAT);
        }
        else if (plantStatus == ItemsList.PlantStatus.Dying)
        {
            water = Mathf.Clamp((water * 0.45f) - 5.0f, 0.0f, MAXSTAT);
            hunger = Mathf.Clamp((hunger * 0.45f) - 5.0f, 0.0f, MAXSTAT);
            affection = Mathf.Clamp((affection * 0.45f) - 5.0f, 0.0f, MAXSTAT);
        }
        else
        {
            water = Mathf.Clamp((water * 0.5f) - 7.0f, 0.0f, MAXSTAT);
            hunger = Mathf.Clamp((hunger * 0.5f) - 7.0f, 0.0f, MAXSTAT);
            affection = Mathf.Clamp((affection * 0.5f) - 7.0f, 0.0f, MAXSTAT);
        }
        
        plantStage++;


    }

    public void UpdateCurrentStats(PlantCreature currentPlant)
    {
        water = currentPlant.Water;
        hunger = currentPlant.Hunger;
        affection = currentPlant.Affection;
        plantLocation = currentPlant.transform;
        plantName = currentPlant.PlantName;
        plantStage = currentPlant.PlantStage;
        plantStatus = currentPlant.PlantStatus;
        plantLight = currentPlant.CurrentMode;


    }
}
