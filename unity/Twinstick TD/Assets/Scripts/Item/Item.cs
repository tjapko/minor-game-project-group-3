using UnityEngine;
using System.Collections;

[SerializeField]
public class Item {
    //Public variables
    public string itemname;
    public int itemID;
    public string itemdescription;
    public Texture2D itemicon;
    public ItemType itemtype;
    
    //Enum of itemtype 
    public enum ItemType
    {
        Weapon,
        Consumable,
        Empty
    }

    //Constructor
    public Item (string name, int id, string description, string iconname, ItemType type)
    {
        itemname = name;
        itemID = id;
        itemdescription = description;
        //itemicon = Resources.Load<Texture2D>("Icons/ItemIcons/" + iconname);
        itemtype = type;
    }

    //Constructor for empty slots
    public Item () {
        itemtype = ItemType.Empty;
    }

    //Compare items
    public bool equals(Item other)
    {
        return other.itemID == this.itemID;

    }
}
