using UnityEngine;
using System.Collections;

public class Upgrade_BaseHealth : BaseUpgrade {

    //Private variable
    private int[] m_basehealthinc;

    //Constructor
    //m_base: the base (gameobject)
    public Upgrade_BaseHealth(int[] price, int[] incHealthAmount) : base(price)
    {
        //Error message
        if (price.Length != incHealthAmount.Length)
        {
            Debug.Log("price.Length != incHealthAmount.Length");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.BaseHealthUpgrade;
        m_basehealthinc = incHealthAmount;
        setIcon("Upgrade_BaseHealthIcon");
    }

    //Upgrade the base
    public void UpgradeBase(GameObject m_base)
    {
        //Check if base contains BaseHealth script
        Basehealth basehealthScript = m_base.GetComponent<Basehealth>();
        if (basehealthScript != null)
        {
            //Inrease basehealth
            basehealthScript.incMaxHealth(m_basehealthinc[selected_index]);
            //Increase index
            incIndex();
        }
    }
}
