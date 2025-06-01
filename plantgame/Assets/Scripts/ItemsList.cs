using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsList
{
    public enum FoodItems: int
    {
        None,
        Fertilizer = 20,
        Fly = 15,
        MealWorm = 10,
        EggShell = 7,
        Steak = 25

    }

    public enum PlantStatus
    {
        Happy,
        Neutral,
        Sad,
        Wilting,
        Dry,
        Dying
    }

    public enum LightMode
    {
        None,
        Low,
        Medium,
        High
    }

}
