using UnityEngine;
using System.Collections;

public class BaseUpgrade {
    //Private variables
    protected BaseUpgradeType upgrade_type;
    protected int[] price_array;
    protected int selected_index;
    protected Sprite upgrade_icon;

    //Enum of upgradetype 
    public enum BaseUpgradeType
    {
        PlayerHealthUpgrade,
        BaseUpgrade,
        TurretDamageUpgrade,
        //TurretSpeedUpgrade,
        //TurretAccuracyUpgrade,
        RestoreBaseHealth,
        RestorePlayerHealth,
        Empty
    }

    //Constructor
    public BaseUpgrade(int[] price)
    {
        this.price_array = price;
        this.upgrade_icon = Resources.Load<Sprite>("Icons/UpgradeIcon/unknown");
        selected_index = 0;
    }

    //Inc index
    public void incIndex()
    {
        if(selected_index < price_array.Length)
        {
            selected_index++;
        }
    }

  
    //Get price of next upgrade, '-1' means no more upgrades
    public int getPrice()
    {
        return selected_index < price_array.Length ? price_array[selected_index] : -1;
    }

    //Setter for iconname
    public void setIcon(string icon_name)
    {
        try
        {
            this.upgrade_icon = Resources.Load<Sprite>("Icons/UpgradeIcon/" + icon_name);
        }
        catch
        {
            this.upgrade_icon = Resources.Load<Sprite>("Icons/UpgradeIcon/unknown");
        }
    }

    //Getter for icon
    public Sprite getIcon()
    {
        return upgrade_icon;
    }

    //Getter for base upgrade type
    public BaseUpgradeType getBaseUpgradeType()
    {
        return upgrade_type;
    }

    //Reset index
    protected void resetIndex()
    {
        selected_index = 0;
    }

}
