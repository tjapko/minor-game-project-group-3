﻿using UnityEngine;
using System.Collections;

public class Upgrade_Base : BaseUpgrade {

    //Private variable
    private int[] m_basehealthinc;
	private int[] m_BaseTurretDamageInc;
	private float[] m_BaseTurretRangeInc;
	private float[] m_BaseTurretAccuracyInc;
	private float[] m_BaseTurretFireRateInc;


    //Constructor
    //m_base: the base (gameobject)
	public Upgrade_Base(int[] price, int[] incHealthAmount, int[] incTurretDamage, float[]  incTurretRange, float[] incTurretAccuracy, float[]  incTurretFireRate) : base(price)
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
        setIcon("Upgrade_BaseHealthIcon");
    }

    //Upgrade the base
    public void UpgradeBase(GameObject m_base)
    {
        //Check if base contains BaseHealth script
        Basehealth basehealthScript = m_base.GetComponent<Basehealth>();
		//Check if base contains BaseTurret script
		BaseTurret baseTurretScript = GameObject.FindGameObjectWithTag("baseTurret").GetComponent<BaseTurret>();

		if (basehealthScript != null && baseTurretScript != null)
        {
            //Inrease basehealth
            basehealthScript.incMaxHealth(m_basehealthinc[selected_index]);
            
	        //Update Slider
	        basehealthScript.SetHealthUI();

			//Inrease turret stats 
			baseTurretScript.setDamage(m_BaseTurretDamageInc[selected_index]);
			baseTurretScript.setRange(m_BaseTurretRangeInc[selected_index]);
			baseTurretScript.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
			baseTurretScript.setFirerate(m_BaseTurretFireRateInc[selected_index]);
			Debug.Log (baseTurretScript.m_damage);
			Debug.Log (baseTurretScript.m_range);
			Debug.Log (baseTurretScript.m_accuracy);
			Debug.Log (baseTurretScript.m_fireRate);

			//Increase index
			incIndex();
		}
			
    }
}
