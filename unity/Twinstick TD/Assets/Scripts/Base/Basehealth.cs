using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Basehealth : MonoBehaviour {

    public float m_StartingHealth;
    public float m_maxhealth;
	private Color m_FullHealthColor = Color.green;
    private Color m_ZeroHealthColor = Color.red;

    private float m_CurrentHealth;
    private bool m_Dead;
	int count;	//number of times enemy hit the base

	void SetUp(){
		count = 0;
	}

	//gets called every time something hits the base and enemy or enemyShell will be set inactive
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Enemy")) {
			other.gameObject.SetActive (false);
			count++;
			TakeDamage (1f);	//maybe not 1 for every kind of enemy
		}
		if (other.gameObject.CompareTag ("EnemyShell")){
			other.gameObject.SetActive (false);
			TakeDamage (1f); 	//if every shell takes 1 health off of healthbase
		}
	}


    // OnEnable
    private void OnEnable()
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

        gameObject.SetActive(false);
    }
}
