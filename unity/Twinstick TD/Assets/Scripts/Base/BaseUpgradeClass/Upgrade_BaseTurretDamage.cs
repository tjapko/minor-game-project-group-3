using UnityEngine;
using System.Collections;

public class Upgrade_BaseTurretDamage : BaseUpgrade {

    //Private variable
    private int[] m_BaseTurretDamageInc;

    //Constructor
    //m_base: the base (gameobject)
    public Upgrade_BaseTurretDamage(int[] price, int[] incTurretDMG) : base(price)
    {
        //Error message
        if (price.Length != incTurretDMG.Length)
        {
            Debug.Log("price.Length != incHealthAmount.Length");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.TurretDamageUpgrade;
        m_BaseTurretDamageInc = incTurretDMG;
        setIcon("Upgrade_BaseTurretDamageIcon");
    }

    //Upgrade the base
    public void upgradeTurretDamage(GameObject turret)
    {
        BaseTurret baseturret_script = turret.GetComponent<BaseTurret>();

        if (baseturret_script != null)
        {

            //Inrease basehealth
            baseturret_script.incTurretDamage(m_BaseTurretDamageInc[selected_index]);
            //Increase index
            incIndex();
        }
    }
}
