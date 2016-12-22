using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class PlayerInventory
/// </summary>
public class PlayerInventory : MonoBehaviour {
    //Public variables
    public int maxslots;
    public List<Weapon> inventory;
    public Rigidbody m_Bullet;
    public Transform m_FireTransform;
    public Rigidbody m_RayBullet;
    //Fill the inventory when the player is initialized
    void Start ()
    {
        inventory = new List<Weapon>();
        //.................. Weapon(name              , id, description      , iconname , price , itemtype               ,  fireratef , launchforcef , maxDamagef, reloadTimef, clipsize ,  ammo , ammopriceperclip, ammoInClip, maxAmmo, lifetime)
        Weapon weapon1 = new Weapon("Default Weapon"  , 1 , "Default weapon!", "Weapon1", 100   , Weapon.ItemType.HandGun, 7f      , 75f          , 0.5f       , 0.5f       , 20        , 9999    , 50				,20 		,500     , 2f);
		Weapon weapon2 = new Weapon("Default Weapon 2", 2 , "Default weapon!", "shotgun", 200   , Weapon.ItemType.Shotgun, 1f         , 50f          , 2f       , 0.5f       , 10        , 20    , 30				,10			,200     , 0.04f);
        Weapon weapon3 = new Weapon("Default Weapon 3", 3 , "Default weapon!", "Weapon3", 300   , Weapon.ItemType.MachineGun , 7f    , 50f         , 10f      , 1.0f       , 60        , 120   , 20               ,60         , 50     , 2f);
        //Weapon sniper = new Weapon("Default Weapon 3", 3 , "Default weapon!", "Weapon3", 300   , Weapon.ItemType.Sniper , 0.75f      , 100f         , 10f      , 1.0f       , 5         , 10    , 20				,5 			,50      , 2f);

        //First add default
        inventory.Add(weapon1);
        //inventory.Add(weapon2);
        //inventory.Add(weapon3);

        //Fill up with empty items
        while (inventory.Count < maxslots)
        {
            inventory.Add(new Weapon());
        }
    }

    //Function update
    void Update()
    {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0 || Input.GetKeyDown("e"))
        {
			swapUp();
        }

		if (scroll < 0 || Input.GetKeyDown("q"))
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

    public void reset()
    {
        Start();
    }
}
