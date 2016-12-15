using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class PlayerInventory
/// </summary>
public class PlayerInventory : MonoBehaviour {
    //Public variables
    public int maxslots;
    public List<Weapon> inventory = new List<Weapon>();
    public Rigidbody m_Bullet;
    public Transform m_FireTransform;
    public Rigidbody m_RayBullet;
    //Fill the inventory when the player is initialized
    void Start ()
    {
        //First add default // weapon(name , id , description , iconname ,price , itemtype,  fireratef , launchforcef , maxDamagef, reloadTimef,clipsize ,  ammo , ammoInClip, bullet , m_bullet)
        Weapon weapon1 = new Weapon("Default Weapon", 1, "Default weapon!", "Weapon1",0 , Weapon.ItemType.HandGun,  1000f, 5f, 30f, 2f , 8 , 30 , 8);
        Weapon weapon2 = new Weapon("Default Weapon 2", 2, "Default weapon!", "Weapon2", 1001, Weapon.ItemType.Shotgun, 1f, 5f, 25f, 3f, 10, 50, 10);
        Weapon weapon3 = new Weapon("Default Weapon 3", 3, "Default weapon!", "Weapon3", 100, Weapon.ItemType.Sniper, 0.75f, 100f, 100f, 2f, 5, 40, 5);
        inventory.Add(weapon1);
        inventory.Add(weapon2);
        inventory.Add(weapon3);

        //Fill up with empty items
        while (inventory.Count < maxslots)
        {
            inventory.Add(new Weapon());
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
    public void addItem(Weapon additem)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            //Check for duplicate items
            if (additem.equals(inventory[i]))
            {
                break;
            }

            //Check for empty items
            if(inventory[i].itemtype.Equals(Weapon.ItemType.Empty))
            {
                inventory[i] = additem;
                break;
            }
        }
    }

    //Function remove item
    public void removeItem(Weapon removeitem)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (removeitem.equals(inventory[i]))
            {
                inventory[i] = new Weapon();
                break;
            }
        }
        reorderInventory();
    }

    //Swap items of index by -1
    public void swapDown()
    {
        if(inventory[1].itemtype != Weapon.ItemType.Empty)
        {
            Weapon firstelement = inventory[0];
            inventory.RemoveAt(0);
            inventory.Add(firstelement);
        }

    }

    //Swaps items by +1
    public void swapUp()
    {
        if(inventory[inventory.Count-1].itemtype != Weapon.ItemType.Empty)
        {
            Weapon firstitem = inventory[0];
            //While the first item hasn't been moved up one spot
            while (!inventory[1].equals(firstitem))
            {
                Weapon temp = inventory[0];
                inventory.RemoveAt(0);
                inventory.Add(temp);
            }
        }
    }

    //Checks if inventory contans the item
    public bool InventoryContains(Weapon otheritem)
    {
        foreach(Weapon inv_item in inventory)
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
        List<Weapon> copy = inventory;
        inventory = new List<Weapon>();

        //First add exisiting items
        foreach (Weapon copy_item in copy)
        {
            if(copy_item.itemtype != Item.ItemType.Empty)
            {
                inventory.Add(copy_item);
            }
        }

        //Fill up with empty items
        while(inventory.Count < maxslots)
        {
            inventory.Add(new Weapon());
        }
    }

    
}
