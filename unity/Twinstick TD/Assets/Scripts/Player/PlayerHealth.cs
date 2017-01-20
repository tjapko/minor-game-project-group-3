using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    //References
    public GameObject m_hitFriendlyCanvasPrefab;    //Reference to friendlyHit canvas;
    
    private GameObject m_maincamera;                //Reference to maincamera

    //audio
    public AudioSource beatSource;                  //Reference to audio heartbeat 
    public AudioSource painSource;                  // Reference to audiosource for sfx
    public AudioClip pain;
    public AudioClip dead;

    public float m_maxHealth;                           //Max health of player
    public float m_StartingHealth;						//Start health of enemy
	public Slider m_Slider;                           	// The slider to represent how much health the enemy currently has.
	public Image m_FillImage;                           // The image component of the slider.
	public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
	public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
    public bool m_Dead = false;  						// Enemy is dead or not

    //Private variables
    
	private float m_CurrentHealth;  					// Current health of enemy
    private int m_dollarperlife;                      // Amount of currency per life that it cost to buy. 
	

	public void Start()
	{
        m_maincamera = GameObject.FindWithTag("MainCamera");
        m_Dead = false;
        // When the enemy is enabled, reset the enemy's health
        m_CurrentHealth = m_StartingHealth;
		// Update the health slider's value and color.
		SetHealthUI();
        painSource.clip = pain;
    }

    public void fixedUpdate()
    {
        
        if (!beatSource.isPlaying && m_CurrentHealth <= 10) {
           
            beatSource.Play();
        }
        else if (beatSource.isPlaying && m_CurrentHealth > 10)
        {
            beatSource.Stop();
        }
    }


	public void SetHealthUI()
	{
		// Set the slider's value appropriately.
		m_Slider.value = m_CurrentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_maxHealth);
	}

    public void buyHealth()
    {
        PlayerStatistics playerstat = GetComponent<PlayerStatistics>();
        float dif = (m_maxHealth - m_CurrentHealth);
        int cost =(int)(dif * m_dollarperlife);
        if (playerstat.m_currency >= cost)
        {
            playerstat.m_currency -= cost;
            m_CurrentHealth = m_maxHealth;
        }
        else
        {

            int kap = (int)(playerstat.m_currency / m_dollarperlife);
            m_CurrentHealth +=  kap;
            playerstat.m_currency -= kap * m_dollarperlife;

        }
        SetHealthUI();

    }
	//Decrease health of base
	public void takeDamage(float amountSec){
       
        painSource.Play();

        //Create hitmark
        createHitMark(m_hitFriendlyCanvasPrefab, amountSec);

        m_CurrentHealth = m_CurrentHealth - amountSec;
		SetHealthUI ();
		if (m_CurrentHealth <= 0) {
			OnDeath ();
		}
		ScreenShake.ShakeCharacter ();
	}

    // OnDeath
    private void OnDeath()
    {
        painSource.transform.parent = null;
        painSource.clip = dead;
        painSource.Play();
      //  Destroy(painSource.gameObject, 4f );
        
        m_Dead = true;
        gameObject.SetActive(false);
    }

    //Spawn hitmark
    private void createHitMark(GameObject prefab, float amount)
    {
        //Set hitmark
        GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        HitMarkScript hitbox_script = hitbox.GetComponent<HitMarkScript>();
        hitbox_script.setDamage(amount);
        hitbox_script.setCamera(m_maincamera);
        hitbox_script.lookToCamera();
        hitbox.SetActive(true);
    }

    //Setter for Dollar Per Life
    public void setDollarPerLife(int amount)
    {
        m_dollarperlife = amount;
    }

    //Setter of max health
    public void addMaxHealth(float amount)
    {
        m_maxHealth += amount;
    }

    //Add health
    public void addHealth(float amount)
    {
        m_CurrentHealth += amount;
    }

    //Getter current health
    public float getCurrentHealth()
    {
        return m_CurrentHealth;
    }
}