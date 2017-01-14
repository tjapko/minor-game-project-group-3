﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class EnemyHealth
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    //Notes:
    //OnDeath() : needs a reference to how much the enemy is worth

    //References
    [Header("References")]
    public GameObject m_hitEnemyCanvasPrefab;       //Reference to enemyHit canbas

    public AudioSource enemySource;
    public AudioClip deathSound;
    //Public variables
	
    public Slider m_Slider;                           	// The slider to represent how much health the enemy currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
	[HideInInspector] public UnitPlayer playerUnit;		// Script UnitPlayer to access it
	
    //Private variables
    private float m_CurrentHealth;  					//Current health of enemy
	private bool m_Dead;  								//Enemy is dead or not
	[HideInInspector] public bool basehit;				//Enemy has hit base or not
    private int m_lasthit;          					//Playernumber of last hit
	private PlayerHealth playerhealth;					//playerhealth script
	private Basehealth basehealth;						//Basehealth script
    private float m_damageToTowerSec;					//damage to tower per second
    private float m_towerpersecond;
    private float m_damageToPlayerSec;					//damage to player per second
    private float m_playerPerSecond;
    private float m_StartingHealth;						//Start health of enemy
    private float m_damageDoneToObject = 0f;
    private float m_damageDoneToPlayer = 0f;

    public void Start()
    {
          
        // Update the health slider's value and color.
        SetHealthUI();

	}

	public void OnTriggerEnter(Collider other){
		//if colide with base, damage base and set enemy to inactive
		if (other.gameObject.CompareTag ("Player")) 
		{
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
			if (targetRigidbody) 
			{
				playerhealth = targetRigidbody.GetComponent<PlayerHealth> ();
				InvokeRepeating ("playerDamage", 0f, m_playerPerSecond);
			}
		}
		if (other.gameObject.CompareTag("Base"))
		{
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
			if (targetRigidbody)
			{
				basehealth = targetRigidbody.GetComponent<Basehealth>();
				InvokeRepeating("baseDamage", 0f, m_towerpersecond);
				playerUnit.baseHit = true;
			}
		}
	}

	//Give tower damage
	private void baseDamage(){
        if(basehealth != null)
        {
            basehealth.TakeDamage(m_damageToTowerSec);
            this.m_damageDoneToObject += m_damageToTowerSec;
        }
	}

	//give player damage
	private void playerDamage(){
        if(playerhealth != null)
        {
            playerhealth.takeDamage(m_damageToPlayerSec);
            this.m_damageDoneToPlayer += m_damageToPlayerSec;
        }
	}

    //Spawn hitmark
    private void createHitMark(GameObject prefab, float amount)
    {
        //Set hitmark
        GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        hitbox.GetComponent<HitMarkScript>().setDamage(amount);
    }

    //stop invoke if colliders are not touching anymore
    public void OnTriggerExit (Collider other){
		CancelInvoke ();
	}

    //Take damage
    public void TakeDamage(float amount)
    {
        //Set hitmark
        createHitMark(m_hitEnemyCanvasPrefab, amount);

        // Reduce current health by the amount of damage done.
        m_CurrentHealth -= amount;
        // Change the UI elements appropriately.
        SetHealthUI();

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (m_CurrentHealth <= 0f && !m_Dead)
        {
			CancelInvoke ();
            OnDeath();
        }

    }

    

    // OnDeath
    private void OnDeath()
    {

      

        //Give player money
        GameObject root = GameObject.FindWithTag("Gamemanager");
        GameManager gamemanager = root.GetComponent<GameManager>();
        PlayerManager playermanager = gamemanager.getUserManager().m_playerlist[m_lasthit];

        enemySource.transform.parent = null;
        enemySource.clip = deathSound;
        enemySource.Play();
        Destroy(enemySource.gameObject , 1f);

        if(playermanager != null)
        {
            playermanager.m_stats.addCurrency(50); //Reference needs to be set
            playermanager.m_stats.addKill();
        }

        m_Dead = true;
//		Destroy (playerUnit);
//		Destroy (gameObject);
        gameObject.SetActive(false);
    }

    // Setters

   // public float m_platerPerSecond
    public void setDamageToTowerSec(float damageTotowerSec)
    {
        this.m_damageToTowerSec = damageTotowerSec;
    }

    public void setTowerperSecond(float towerperSecond)
    {
        this.m_towerpersecond = towerperSecond;
    }

    public void setDamageToPlayerSec(float damageToPlayerSec)
    {
        this.m_damageToPlayerSec = damageToPlayerSec;
    }

    public void setPlayerPerSecond(float PlayerPerSecond)
    {
        this.m_playerPerSecond = PlayerPerSecond;
    }

    //Set player number of last hit
    public void setLastHit(int playernumber)
    {
        m_lasthit = playernumber;
    }

    //Set current health of the enemy
    public void setCurrentHealth(float value)
    {
        m_CurrentHealth = value;
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }
}