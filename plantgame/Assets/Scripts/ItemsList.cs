using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsList
{
    public struct Food
    {
        public int Fertilizer;
    }
    // 
    public enum FoodItems: int
    {
        None,
        Fertilizer = 20,
        Fly = 25,
        MealWorm = 15,
        EggShell = 10,
        Steak = 25

    }

    public enum PlantStatus
    {
        Happy,
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

    public void RandomizeItems()
    {

    }
}
