using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Basehealth : MonoBehaviour {

    public float m_StartingHealth;					// Initial health of base
    public float m_maxhealth;						// Maximum health of base
	[HideInInspector] public GameObject tower;		// Base
	private Color m_FullHealthColor = Color.green;	
    private Color m_ZeroHealthColor = Color.red;	

    private float m_CurrentHealth;					// Current health of base
    private bool m_Dead;							// Base dead or not

	void SetUp(){
		m_CurrentHealth = m_StartingHealth;
		m_Dead = false;
	}

    // OnEnable
    public void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
    }

    //Take damage
    public void TakeDamage(float amount)
    {
        if(amount > 0)
        {
            m_CurrentHealth -= amount;
        }

		if (m_CurrentHealth <= 0f && !m_Dead)
        {
			OnDeath();
        }
    }

    //Heal base
    public void Healbase(float amount)
    {
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
		tower.SetActive(false);
    }
}
