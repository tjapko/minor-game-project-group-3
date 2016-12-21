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

        //.................. Weapon(name              , id, description      , iconname , price , itemtype               ,  fireratef , launchforcef , maxDamagef, reloadTimef, clipsize ,  ammo , ammoInClip)
        Weapon weapon1 = new Weapon("Default Weapon"  , 1 , "Default weapon!", "Weapon1", 100   , Weapon.ItemType.HandGun, 10f        , 35f          , 1f       , 0.1f       , 20        , 1000000    , 20);
        Weapon weapon2 = new Weapon("Default Weapon 2", 2 , "Default weapon!", "Weapon2", 200   , Weapon.ItemType.Shotgun, 5.0f       , 50f          , 2f       , 0.5f       , 10        , 1000000    , 10);
        Weapon weapon3 = new Weapon("Default Weapon 3", 3 , "Default weapon!", "Weapon3", 300   , Weapon.ItemType.Sniper , 0.5f       , 100f         , 10f      , 1.0f       , 5         , 1000000    , 5 );

		//First add default
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
