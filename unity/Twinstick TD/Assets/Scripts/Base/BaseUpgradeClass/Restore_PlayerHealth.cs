using UnityEngine;
using System.Collections;

public class Restore_PlayerHealth : BaseUpgrade {
    //Private variable
    private int m_restoreamount;

    //Constructor
    //m_base: the base (gameobject)
    public Restore_PlayerHealth(int[] price, int restoreAmount) : base(price)
    {
        //Error message
        if (price.Length != 1)
        {
            Debug.Log("Price length should be 1");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.RestorePlayerHealth;
        m_restoreamount = restoreAmount;
        setIcon("Restore_PlayerHealthIcon");
    }

    //Restore health of base
    public void restorePlayerHealth(PlayerManager m_player)
    {
        //Might need fix?
        m_player.m_playerhealth.setDollarPerLife((int)((float)price_array[0] / (float)m_restoreamount));
        m_player.m_playerhealth.buyHealth();
        m_player.m_playerhealth.SetHealthUI();
    }


}
