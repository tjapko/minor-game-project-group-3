using UnityEngine;
using System.Collections;

public class Restore_BaseHealth : BaseUpgrade {
    //Private variable
    private int m_restoreamount;
    GameObject m_base;
    //Constructor
    //m_base: the base (gameobject)
    public Restore_BaseHealth(int[] price, int restoreAmount) : base(price)
    {
        //Error message
        if (price.Length != 1)
        {
//            Debug.Log("Price length should be 1");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.RestoreBaseHealth;
        m_restoreamount = restoreAmount;
        setIcon("Restore_BaseHealthIcon");


    }
	private int Cost(GameObject m_base)
    {
//        Basehealth basehealth_script = m_base.GetComponent<Basehealth>();
//        float dif = (basehealth_script.m_maxhealth- basehealth_script.getCurrentHealth());
        return ((int)(price_array[0]));
    }


    //Restore health of base
    public void restoreBaseHealth(GameObject m_base, PlayerManager m_player)
    {
        Basehealth basehealth_script = m_base.GetComponent<Basehealth>();
        PlayerStatistics player_stats = m_player.m_stats;
        if(basehealth_script != null)
        {
            
			int cost = Cost(m_base);

            if (cost <= player_stats.m_currency)
            {
				basehealth_script.Healbase(m_restoreamount);
                player_stats.substractCurrency(cost);
            }

            basehealth_script.SetHealthUI();
        }
        
    }
}
