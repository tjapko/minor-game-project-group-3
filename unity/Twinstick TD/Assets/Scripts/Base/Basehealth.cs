using UnityEngine;
using System.Collections;

/// <summary>
/// Class Basehealh
/// </summary>
public class Basehealth : MonoBehaviour {

    //Public variables
    public float m_StartingHealth = 100f;           //Starting health
    public float m_maxhealth = 100f;                //Maximum health
    public Color m_FullHealthColor = Color.green;   //Full health colour
    public Color m_ZeroHealthColor = Color.red;     //Zero health colour

    private float m_CurrentHealth;                  //Current health of tower
    private bool m_Dead;                            //Boolean if tower is dead

    // OnEnable
    private void OnEnable()
    {   
        //Set starting variables
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
    }

    //Take damage function
    public void TakeDamage(float amount)
    {
        //Amount must be smaller than zero
        if(amount < 0)
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

    // OnDeath
    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        gameObject.SetActive(false);
    }
}
