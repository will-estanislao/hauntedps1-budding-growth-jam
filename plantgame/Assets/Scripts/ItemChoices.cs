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

    private bool isDragging = false;

    private float CAMDISTANCE = 15.0f;

    private void Awake()
    {
        objRigidbody = GetComponent<Rigidbody>();
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
        objRigidbody.useGravity = false;
        // Enable Collision
        
    }

    private void OnMouseUp()
    {
        isDragging = false;
        objRigidbody.useGravity = true;
        //Disable collision
    }

    public void DragObject()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CAMDISTANCE));
        

        //Debug.Log("On Drag" + mousePos + "\nObject Location: " + transform.position);
        

        objRigidbody.MovePosition(new Vector3(mousePos.x, mousePos.y, 0.0f));
        //transform.localPosition = new Vector3(mousePos.x - startXPos, mousePos.y - startYPos, 0.0f);
    }
}
