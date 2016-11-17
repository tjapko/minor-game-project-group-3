using UnityEngine;
using System.Collections;

public class Basehealth : MonoBehaviour {

    public float m_StartingHealth = 100f;
    public float m_maxhealth = 100f;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;

    private float m_CurrentHealth;
    private bool m_Dead;

    // OnEnable
    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
    }

    //Take damage
    public void TakeDamage(float amount)
    {
        if(amount < 0)
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

        gameObject.SetActive(false);
    }
}
