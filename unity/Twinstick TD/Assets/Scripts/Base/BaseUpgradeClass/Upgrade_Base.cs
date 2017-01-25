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
        BaseUpgradeScript baseupgradeScript = m_base.GetComponent<BaseUpgradeScript>();

        if (basehealthScript != null)
        {
            //Inrease basehealth
            basehealthScript.incMaxHealth(m_basehealthinc[selected_index]);

            //Update Slider
            basehealthScript.SetHealthUI();
        }

        //Inrease base turret stats 
        BaseTurret.setDamage(m_BaseTurretDamageInc[selected_index]);
        BaseTurret.setRange(m_BaseTurretRangeInc[selected_index]);
        BaseTurret.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
        BaseTurret.setFirerate(m_BaseTurretFireRateInc[selected_index]);
        BaseTurret.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
        BaseTurret.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);

        //Increase player turret stats
        TurretScript.setDamage(m_BaseTurretDamageInc[selected_index]);
        TurretScript.setRange(m_BaseTurretRangeInc[selected_index]);
        TurretScript.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
        TurretScript.setFirerate(m_BaseTurretFireRateInc[selected_index]);
        TurretScript.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
        TurretScript.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);

        //Spawn turrets
        checkBaseTurret(selected_index, baseupgradeScript);

        //Increase index
        incIndex();
}

	public void revertUpgrade(GameObject m_base)
	{
		//Reset index
		resetIndex();

        //Inrease turret stats 
        BaseTurret.setDamage(m_BaseTurretDamageInc[selected_index]);
        BaseTurret.setRange(m_BaseTurretRangeInc[selected_index]);
        BaseTurret.setAccuracy(m_BaseTurretAccuracyInc[selected_index]);
        BaseTurret.setFirerate(m_BaseTurretFireRateInc[selected_index]);
        BaseTurret.setLaunchForce(m_BaseTurretLaunchSpeedInc[selected_index]);
        BaseTurret.setTurnRate(m_BaseTurretTurnRateInc[selected_index]);

        m_base.GetComponent<BaseUpgradeScript>().resetTurrets();


    }

    //Switch statement for placing turrets
    private void checkBaseTurret(int upgrade, BaseUpgradeScript script)
    {
        switch (upgrade)
        {
            case 0:
                spawnBaseTurret(script, 1);
                break;
            case 1:
                spawnBaseTurret(script, 2);
                break;
            default:
                break;
        }

    }
    //Placing turrets
    private void spawnBaseTurret(BaseUpgradeScript script, int active_turrets)
    {
        if (script.m_baseturrets != null && script.m_baseturrets.Count > 0)
        {
            int turret_amount = script.m_baseturrets.Count;
            for (int i =0; i < turret_amount; i++)
            {
                if(i < active_turrets)
                {
                    script.m_baseturrets[i].SetActive(true);
                }
            }
        }
    }
}
