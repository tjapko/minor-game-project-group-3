using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class EnemyHealth
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    //Notes:
    //OnDeath() : needs a reference to how much the enemy is worth

    //Public variables
    public AudioClip dyingSound;                        // sound of dying bunny
    public AudioSource audioSource;                     // AudioSource

	public float m_damageToTowerSec;
	public float m_damageToPlayerSec;
	public float m_StartingHealth;						//Start health of enemy
    public Slider m_Slider;                           	// The slider to represent how much health the enemy currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
	[HideInInspector] public UnitPlayer playerUnit;		// Script UnitPlayer to access it
	//Private variables
    private float m_CurrentHealth;  					//Current health of enemy
	private bool m_Dead;  								//Enemy is dead or not
	[HideInInspector] public bool basehit;				//Enemy has hit base or not
    private int m_lasthit;          //Playernumber of last hit
	private PlayerHealth playerhealth;
	private Basehealth basehealth;

    public void Start()
    {
        // When the enemy is enabled, reset the enemy's health
        m_CurrentHealth = m_StartingHealth;
        // Update the health slider's value and color.
        SetHealthUI();
    }

	public void OnTriggerEnter(Collider other){
		//if colide with base, damage base and set enemy to inactive
		if (other.gameObject.CompareTag ("Player")) {
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
			if (targetRigidbody) {
				playerhealth = targetRigidbody.GetComponent<PlayerHealth> ();
				InvokeRepeating ("playerDamage", 0f, 1f);
			}	
		}
		if (other.gameObject.CompareTag("Base"))
		{
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
			if (targetRigidbody)
			{
				basehealth = targetRigidbody.GetComponent<Basehealth>();
				InvokeRepeating("baseDamage", 0f, 1f);

				//if it has to go to player dont die, otherwise OnDeath()
				//if (playerUnit.playerFirst == false) {
				//	playerUnit.goToPlayer ();
				//} else if (playerUnit.playerFirst) {
				//	OnDeath ();
				//}
			}
		}
	}

	private void baseDamage(){
		basehealth.TakeDamage (m_damageToTowerSec);
	}

	private void playerDamage(){
		playerhealth.takeDamage(m_damageToPlayerSec);
	}

	public void OnTriggerExit (Collider other){
		CancelInvoke ();
	}

    //Take damage
    public void TakeDamage(float amount)
    {
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

    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }
    // OnDeath
    private void OnDeath()
    {
        //Give player money
        GameObject root = GameObject.FindWithTag("Gamemanager");
        GameManager gamemanager = root.GetComponent<GameManager>();
        PlayerManager playermanager = gamemanager.getUserManager().m_playerlist[m_lasthit];


        if(playermanager != null)
        {
            playermanager.m_stats.addCurrency(100); //Reference needs to be set
            playermanager.m_stats.addKill();
        }

        m_Dead = true;
        audioSource.transform.parent = null;
        
        audioSource.clip = dyingSound;
        audioSource.Play();
        Destroy(audioSource.gameObject, 1f);
//		Destroy (playerUnit);
//		Destroy (gameObject);
        gameObject.SetActive(false);
	
    }

    //Set player number of last hit
    public void setLastHit(int playernumber)
    {
        m_lasthit = playernumber;
    }
}