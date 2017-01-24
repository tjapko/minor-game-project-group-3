using UnityEngine;
using System.Collections;

public class Upgrade_Base : BaseUpgrade {

    //Private variable
    private int[] m_basehealthinc;
	private int[] m_BaseTurretDamageInc;
	private float[] m_BaseTurretRangeInc;
	private float[] m_BaseTurretAccuracyInc;
	private float[] m_BaseTurretFireRateInc;
	private float[] m_BaseTurretLaunchSpeedInc;
	private float[] m_BaseTurretTurnRateInc;


    //Constructor
    //m_base: the base (gameobject)
	public Upgrade_Base(int[] price, int[] incHealthAmount, int[] incTurretDamage, float[]  incTurretRange, float[] incTurretAccuracy, float[]  incTurretFireRate, float[] incTurretLaunchSpeed, float[] incTurretTurnRate) : base(price)
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
        //Boolean if upgrade was a (partial) success
        bool succes = false;
        
        //Load objects and scripts
        Basehealth basehealthScript = m_base.GetComponent<Basehealth>();
		GameObject baseTurrets = GameObject.FindWithTag("baseTurret");
        GameObject playerTurrets = GameObject.FindWithTag("PlayerTurret");

        //Check if base contains BaseHealth script
        if (basehealthScript != null)
        {
            //Inrease basehealth
            basehealthScript.incMaxHealth(m_basehealthinc[selected_index]);
	        basehealthScript.SetHealthUI(); //Update Slider 
            succes = true;

        }

        //Check if base contains BaseTurret script
        if (baseTurrets != null)
        {
            BaseTurret turret_script = baseTurrets.GetComponent<BaseTurret>();
            if (turret_script != null)
            {
                //Inrease turret stats 
                turret_script.setDamage(m_BaseTurretDamageInc[selected_index]);
                turret_script.setRange(m_BaseTurretRangeInc[selected_index]);
                turret_script.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
                turret_script.setFirerate(m_BaseTurretFireRateInc[selected_index]);
                turret_script.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
                turret_script.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);
                succes = true;
            }
        }

        //Check all player turrets
        if (playerTurrets != null)
        {
            TurretScript turret_script = playerTurrets.GetComponent<TurretScript>();
            if (turret_script != null)
            {
                //Inrease turret stats 
                turret_script.setDamage(m_BaseTurretDamageInc[selected_index]);
                turret_script.setRange(m_BaseTurretRangeInc[selected_index]);
                turret_script.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
                turret_script.setFirerate(m_BaseTurretFireRateInc[selected_index]);
                turret_script.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
                turret_script.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);
                succes = true;

            }
        }

        if (succes)
        {
            incIndex();
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
                turret_script.setDamage(m_BaseTurretDamageInc[selected_index]);
                turret_script.setRange(m_BaseTurretRangeInc[selected_index]);
                turret_script.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
                turret_script.setFirerate(m_BaseTurretFireRateInc[selected_index]);
                turret_script.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
                turret_script.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);
            }
        }


    }
}
