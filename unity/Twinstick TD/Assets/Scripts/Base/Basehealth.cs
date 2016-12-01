using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class Basehealh
/// </summary>
public class Basehealth : MonoBehaviour {

    //Public variables
    public float m_StartingHealth = 100f;           //Starting health
    public float m_maxhealth = 100f;                //Maximum health
    public Color m_FullHealthColor = Color.green;   //Full health colour
    public Color m_ZeroHealthColor = Color.red;     //Zero health colour
    public Slider m_Slider;
    public Image m_FillImage;
    //Private variables
    private float m_CurrentHealth;                  //Current health of tower
    private bool m_Dead;                            //Boolean if tower is dead
    private int count = 0;	                        //number of times enemy hit the base
    
    // OnEnable
    private void OnEnable()
    {   
        //Set starting variables
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
        SetHealthUI();
    }

    //Gets called every time something hits the base and enemy or enemyShell will be set inactive
    void OnTriggerEnter(Collider other)
    {
        
        //Enemy shell
        if (other.gameObject.CompareTag("EnemyShell"))
        {
            other.gameObject.SetActive(false);
            TakeDamage(20f); //Future damage should be a variable of the enemy
        }

    }

    //Take damage function
    public void TakeDamage(float amount)
    {
        SetHealthUI();
        //Amount must be smaller than zero
        if (amount > 0)
        {
            m_CurrentHealth -= amount;
        }
        //Check if base has less than 0 health
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            m_CurrentHealth = 0f;
            OnDeath();
        }
    }

    //Heal base
    public void Healbase(float amount)
    {
        //Heal base to max health
        if(m_CurrentHealth + amount >= m_maxhealth)
        {
            m_CurrentHealth = m_maxhealth;
        } else if (amount > 0)
        {
            m_CurrentHealth += amount;
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
        m_Dead = true;

        gameObject.SetActive(false);
    }
}
