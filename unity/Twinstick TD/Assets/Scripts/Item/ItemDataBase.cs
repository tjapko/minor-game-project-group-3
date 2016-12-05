using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataBase : MonoBehaviour {
    public List<Item> listitem = new List<Item>();

    void Start()
    {
        //Test items
        Item item1 = new Item("Item 1",1,"This is item 1","",Item.ItemType.Weapon);
        listitem.Add(item1);
    }

    //Tries to add item to the itemlist
    public void addItem(Item newitem)
    {
        bool unique = true;
        foreach(Item existingitem in listitem)
        {
            if (newitem.equals(existingitem))
            {
                unique = false;
                break;
            }
        }

        if (unique)
        {
            listitem.Add(newitem);
        } else
        {
            Debug.Log("Item already exists");
        }
    }

    //Tries to remove item from the itemlist
    public void removeItem(Item removeitem)
    {
        bool exists = false;
        for(int i = 0; i < listitem.Count; i++)
        {
            if (removeitem.equals(listitem[i]))
            {
                exists = true;
                listitem.RemoveAt(i);
                break;
            }
        }

        if (!exists)
        {
            Debug.Log("Item does not exists");
        }
    }


}
