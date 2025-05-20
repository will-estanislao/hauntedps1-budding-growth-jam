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
        Fly = 30,
        MealWorm = 15,
        EggShell = 10,
        Steak = 35

    }

    public enum PlantStatus
    {
        Happy,
        Sad,
        Wilting,
        Dry,
        Dying
    }

    public void RandomizeItems()
    {

    }
}
