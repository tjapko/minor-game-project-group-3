using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    //References
    [Header("References")]
    public GameObject m_hitFriendlyCanvasPrefab;    //Reference to friendlyHit canvas;

    public float m_StartingHealth;						//Start health of enemy
	public Slider m_Slider;                           	// The slider to represent how much health the enemy currently has.
	public Image m_FillImage;                           // The image component of the slider.
	public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
	public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
	private float m_CurrentHealth;  					//Current health of enemy
	private bool m_Dead;  								//Enemy is dead or not

	public void Start()
	{
		// When the enemy is enabled, reset the enemy's health
		m_CurrentHealth = m_StartingHealth;
		// Update the health slider's value and color.
		SetHealthUI();
	}

	private void SetHealthUI()
	{
		// Set the slider's value appropriately.
		m_Slider.value = m_CurrentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	}

    //Take damage function
	public void takeDamage(float amountSec){
        createHitMark(m_hitFriendlyCanvasPrefab, amountSec);
        m_CurrentHealth =+ -amountSec;
	}

    //Spawn hitmark
    private void createHitMark(GameObject prefab, float amount)
    {
        //Set hitmark
        GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        hitbox.GetComponent<HitMarkScript>().setDamage(amount);
    }
}