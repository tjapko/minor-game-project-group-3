using UnityEngine;
using System.Collections;

public class Upgrade_PlayerHealth : BaseUpgrade {
    //Private variable
    private int[] m_upgradeAmount;

    //Constructor
    //m_base: the base (gameobject)
    public Upgrade_PlayerHealth(int[] price, int[] upgradeAmount) : base(price)
    {
        //Error message
        if (price.Length != upgradeAmount.Length)
        {
            Debug.Log("price.Length should be upgradeAmount.Length");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.PlayerHealthUpgrade;
        m_upgradeAmount = upgradeAmount;
        setIcon("Upgrade_PlayerHealthIcon");
    }

    //Restore health of base
    public void upgradePlayerHealth(PlayerManager m_player)
    {
        m_player.m_playerhealth.setMaxHealth(m_upgradeAmount[selected_index]);
        incIndex();

    }

}
