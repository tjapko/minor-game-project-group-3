using UnityEngine;
using System.Collections;

public class Restore_BaseHealth : BaseUpgrade {
    //Private variable
    private int m_restoreamount;

    //Constructor
    //m_base: the base (gameobject)
    public Restore_BaseHealth(int[] price, int restoreAmount) : base(price)
    {
        //Error message
        if (price.Length != 1)
        {
            Debug.Log("Price length should be 1");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.RestoreBaseHealth;
        m_restoreamount = restoreAmount;
        setIcon("Restore_BaseHealthIcon");

    }

    //Restore health of base
    public void restoreBaseHealth(GameObject m_base, PlayerManager m_player)
    {
        Basehealth basehealth_script = m_base.GetComponent<Basehealth>();
        PlayerStatistics player_stats = m_player.m_stats;
        if(basehealth_script != null)
        {
            float dif = (basehealth_script.m_maxhealth - basehealth_script.getCurrentHealth());
            int cost = (int)((dif/ m_restoreamount) * (float) price_array[0]);

            if (cost <= player_stats.m_currency)
            {
                basehealth_script.Healbase(dif);
                player_stats.substractCurrency(cost);
            }
            else
            {
                int cap = (int)((player_stats.m_currency / (float)price_array[0]) * m_restoreamount);
                basehealth_script.Healbase(cap);
                player_stats.substractCurrency(player_stats.m_currency);

            }
        }
        
    }
}
