using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemChoices : MonoBehaviour
{

    // In main pick up and drag objects in its update.
    // Item type - To pass thru to plant to check
    [SerializeField]
    public int itemType;
    [SerializeField]
    public ItemsList.FoodItems foodType;

    Rigidbody objRigidbody;
    SpriteRenderer sprite;
    Sprite spriteObJ;

    private bool isDragging = false;

    private float CAMDISTANCE = 15.0f;

    private void Awake()
    {
        objRigidbody = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();

        spriteObJ = LoadAsset(FindFileName(foodType));

        sprite.sprite = spriteObJ;
        
    }

    public void OnUpdate()
    {
        if(isDragging)
        {
            DragObject();
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        //objRigidbody.gravityScale = 0;
        objRigidbody.useGravity = false;

    }

    private void OnMouseUp()
    {
        isDragging = false;
        //objRigidbody.gravityScale = 1;
        objRigidbody.useGravity = true;
    }

    private string FindFileName(ItemsList.FoodItems foodtype)
    {
        if(foodType == ItemsList.FoodItems.Steak)
        {
            return "meat";
        }
        else if (foodType == ItemsList.FoodItems.MealWorm)
        {
            return "worm";
        }
        else if (foodType == ItemsList.FoodItems.Fly)
        {
            return "fly";
        }
        else if (foodType == ItemsList.FoodItems.EggShell)
        {
            return "eggshell";
        }
        else if (foodType == ItemsList.FoodItems.Fertilizer)
        {
            return "fertilizer";
        }
        else if (foodType == ItemsList.FoodItems.None)
        {
            return "wateringcan";
        }
        else
        {
            return "";
        }
    }

    private Sprite LoadAsset(string filename)
    {
        string file = "Sprites/" + filename;
        Debug.Log("Trying to load Prefab from file (" + filename + ")...");
        Sprite loadedObject = Resources.Load<Sprite>(file);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    public void DragObject()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CAMDISTANCE));
        

        //Debug.Log("On Drag" + mousePos + "\nObject Location: " + transform.position);
        

        objRigidbody.MovePosition(new Vector3(mousePos.x, mousePos.y, 0.0f));
        //transform.localPosition = new Vector3(mousePos.x - startXPos, mousePos.y - startYPos, 0.0f);
    }
}
