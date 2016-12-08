using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class PlayerInventory
/// </summary>
public class PlayerInventory : MonoBehaviour {
    //Public variables
    public int maxslots;
    public List<Item> inventory = new List<Item>();

    //Fill the inventory when the player is initialized
    void Start ()
    {
        //First add default weapon
        Item weapon1 = new Item("Default Weapon", 1, "Default weapon!", "Weapon1", Item.ItemType.Weapon);
        //Item weapon2 = new Item("Default Weapon 2", 2, "Default weapon!", "Weapon2", Item.ItemType.Weapon);
        //Item weapon3 = new Item("Default Weapon 3", 3, "Default weapon!", "Weapon3", Item.ItemType.Weapon);
        inventory.Add(weapon1);
        //inventory.Add(weapon2);
        //inventory.Add(weapon3);

        //Fill up with empty items
        while (inventory.Count < maxslots)
        {
            inventory.Add(new Item());
        }
    }

    //Function update
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            swapUp();
        }

        if (Input.GetKeyDown("e"))
        {
            swapDown();
        }
    }


    //Function add item
    public void addItem(Item additem)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            //Check for duplicate items
            if (additem.equals(inventory[i]))
            {
                break;
            }

            //Check for empty items
            if(inventory[i].itemtype.Equals(Item.ItemType.Empty))
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
        reorderInventory();
    }

    //Swap items of index by -1
    public void swapDown()
    {
        if(inventory[1].itemtype != Item.ItemType.Empty)
        {
            Item firstelement = inventory[0];
            inventory.RemoveAt(0);
            inventory.Add(firstelement);
        }

    }

    //Swaps items by +1
    public void swapUp()
    {
        if(inventory[inventory.Count-1].itemtype != Item.ItemType.Empty)
        {
            Item firstitem = inventory[0];
            //While the first item hasn't been moved up one spot
            while (!inventory[1].equals(firstitem))
            {
                Item temp = inventory[0];
                inventory.RemoveAt(0);
                inventory.Add(temp);
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

    //Re-order inventory
    private void reorderInventory()
    {
        List<Item> copy = inventory;
        inventory = new List<Item>();

        //First add exisiting items
        foreach (Item copy_item in copy)
        {
            if(copy_item.itemtype != Item.ItemType.Empty)
            {
                inventory.Add(copy_item);
            }
        }

        //Fill up with empty items
        while(inventory.Count < maxslots)
        {
            inventory.Add(new Item());
        }
    }

    
}
