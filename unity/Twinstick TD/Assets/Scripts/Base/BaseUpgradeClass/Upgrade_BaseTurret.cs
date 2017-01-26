using UnityEngine;
using System.Collections;

public class Upgrade_BaseTurret : BaseUpgrade {

	//Private variable
	private int[] m_BaseTurretDamage;

    //Constructor
    //m_base: the base (gameobject)
    public Upgrade_BaseTurret(int[] price, int[] incTurretDMG) : base(price)
    {
        //Error message
        if (price.Length != incTurretDMG.Length)
        {
//            Debug.Log("price.Length != incHealthAmount.Length");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.TurretDamageUpgrade;
        m_BaseTurretDamage = incTurretDMG;
        setIcon("Upgrade_BaseTurretDamageIcon");
    }

}
