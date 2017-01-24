using UnityEngine;
using System.Collections;

public class Upgrade_Base : BaseUpgrade {

    //Private variable
    private int[] m_basehealthinc;
	private float[] m_BaseTurretDamageInc;
	private float[] m_BaseTurretRangeInc;
	private float[] m_BaseTurretAccuracyInc;
	private float[] m_BaseTurretFireRateInc;
	private float[] m_BaseTurretLaunchSpeedInc;
	private float[] m_BaseTurretTurnRateInc;


    //Constructor
    //m_base: the base (gameobject)
	public Upgrade_Base(int[] price, int[] incHealthAmount, float[] incTurretDamage, float[]  incTurretRange, float[] incTurretAccuracy, float[]  incTurretFireRate, float[] incTurretLaunchSpeed, float[] incTurretTurnRate) : base(price)
    {
        //Error message
        if (price.Length != incHealthAmount.Length)
        {
            Debug.Log("price.Length != incHealthAmount.Length");
        }

        //Set variables
        upgrade_type = BaseUpgradeType.BaseUpgrade;
        m_basehealthinc = incHealthAmount;
		m_BaseTurretDamageInc = incTurretDamage;
		m_BaseTurretRangeInc = incTurretRange;
		m_BaseTurretAccuracyInc = incTurretAccuracy;
		m_BaseTurretFireRateInc = incTurretFireRate;
		m_BaseTurretLaunchSpeedInc = incTurretLaunchSpeed;
		m_BaseTurretTurnRateInc = incTurretTurnRate;

        setIcon("Upgrade_BaseHealthIcon");
    }

    //Upgrade the base
	public void UpgradeBase(GameObject m_base)
    {
        //Check if base contains BaseHealth script
        Basehealth basehealthScript = m_base.GetComponent<Basehealth>();

		if (basehealthScript != null) {
			//Inrease basehealth
			basehealthScript.incMaxHealth (m_basehealthinc [selected_index]);
            
			//Update Slider
			basehealthScript.SetHealthUI ();

			//Inrease turret stats 
			BaseTurret.setDamage (m_BaseTurretDamageInc [selected_index]);
			BaseTurret.setRange (m_BaseTurretRangeInc [selected_index]);
			BaseTurret.setAccuracy (m_BaseTurretAccuracyInc [selected_index]);
			BaseTurret.setFirerate (m_BaseTurretFireRateInc [selected_index]);
			BaseTurret.setLaunchForce (m_BaseTurretLaunchSpeedInc [selected_index]);
			BaseTurret.setTurnRate (m_BaseTurretTurnRateInc [selected_index]);
			Debug.Log ("getDamage: " + BaseTurret.getDamage ());
			//Increase index
			incIndex ();
		}
    }

	public void revertUpgrade()
	{
		//Reset index
		resetIndex();

		//Load objects and scripts
		GameObject baseTurrets = GameObject.FindWithTag("baseTurret");
		//Check if base contains BaseTurret script
		if (baseTurrets != null)
		{
			BaseTurret turret_script = baseTurrets.GetComponent<BaseTurret>();
			if (turret_script != null)
			{
				//Inrease turret stats 
				BaseTurret.setDamage(m_BaseTurretDamageInc[selected_index]);
				BaseTurret.setRange(m_BaseTurretRangeInc[selected_index]);
				BaseTurret.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
				BaseTurret.setFirerate(m_BaseTurretFireRateInc[selected_index]);
				BaseTurret.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
				BaseTurret.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);
			}
		}

	}

}
