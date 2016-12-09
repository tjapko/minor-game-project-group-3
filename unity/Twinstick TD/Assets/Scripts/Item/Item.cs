using UnityEngine;
using System.Collections;

[SerializeField]
public class Item {
    //Public variables
    public string itemname;
    public int itemID;
    public string itemdescription;
    public Sprite itemicon;
    public int itemprice;
    public ItemType itemtype;
    
    //Enum of itemtype 
    public enum ItemType
    {
        Weapon,
        Consumable,
        Empty
    }

    //Constructor
    public Item (string name, int id, string description, string iconname, int price, ItemType type)
    {
        itemname = name;
        itemID = id;
        itemdescription = description;
        itemprice = price;
        itemicon = Resources.Load<Sprite>("Icons/ItemIcons/" + iconname);
        itemtype = type;
    }

    //Constructor for empty slots
    public Item () {
        itemtype = ItemType.Empty;
        itemicon = Resources.Load<Sprite>("Icons/ItemIcons/noweapon");
    }

    //Compare items
    public bool equals(Item other)
    {
        return other.itemID == this.itemID;

    }
}
