using UnityEngine;
using System.Collections;

public class Upgrade_Empty : BaseUpgrade
{
    //Constructor
    //m_base: the base (gameobject)
    public Upgrade_Empty(int[] price):base(price)
    {
        price_array = new int[] { -1 };
        //Set variables
        upgrade_type = BaseUpgradeType.Empty;
        setIcon("unknown");
    }
}
