using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class PlayerInventory
/// Source: https://www.youtube.com/watch?v=gEfKhGtqU70
/// </summary>
public class PlayerInventory : MonoBehaviour {
    //Public variables
    public int slotsX, slotsY;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();

    //References
    public ItemDataBase itemdatabase;
    public GUISkin skin;

    //Private variables
    private bool showinventory = false;
    private bool showtooltip = false;
    private string tooltiptext;

    //Fill the inventory when the player is initialized
    void start ()
    {
        for(int i = 0; i < slotsX * slotsY; i++)
        {
            inventory.Add(new Item());
            slots.Add(new Item());
        }
    }

    //Update function
    void Update()
    {
        if(Input.GetButtonDown("i"))
        {
            showinventory = !showinventory;
        }
    }

    //Function add item
    public void addItem(Item additem)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemtype != Item.ItemType.Empty)
            {
                inventory[i] = additem;
                break;
            }
        }
    }

    //Function remove item
    public void removeItem(Item removeitem)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (removeitem.equals(inventory[i]))
            {
                inventory[i] = new Item();
                break;
            }
        }
    }

    //Checks if inventory contans the item
    private bool InventoryContains(Item otheritem)
    {
        foreach(Item inv_item in inventory)
        {
            if (otheritem.equals(inv_item))
            {
                return true;
            }
        }
        return false;
    }


    //GUI
    void OnGUI()
    {
        if (showinventory)
        {
            drawinventory();
        }

        if(showtooltip)
        {
            Rect rect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200,200);
            GUI.Box(rect, tooltiptext);
            showtooltip = false;
        }
    }

    // Creates tooltip
    private void createtooltip(Item item)
    {
        tooltiptext = "";
        tooltiptext += item.itemdescription;

    }
    //Function to draw inventory onto screen
    private void drawinventory()
    {
        int item_index = 0;

        //i and j are used for positioning the UI
        for (int y = 0; y < slotsY; y++)
        {
            for (int x = 0; x < slotsX; x++)
            {
                Rect rect = new Rect(x * 50, y * 50, 50, 50);
                GUI.Box(rect, "", skin.GetStyle("Slot"));

                if (inventory[item_index].itemtype != Item.ItemType.Empty)
                {
                    GUI.DrawTexture(rect, slots[item_index].itemicon);
                }

                if (rect.Contains(Event.current.mousePosition))
                {
                    createtooltip(slots[item_index]);
                    showtooltip = true;
                }


                item_index++;
            }
        }
    }

    //Function to draw tooltip
    private void drawtooltip()
    {

    }
    


}
