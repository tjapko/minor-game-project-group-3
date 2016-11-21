using UnityEngine;
using System.Collections;

/// <summary>
/// Class EnemyHealth
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    //Public variables
    public float m_StartingHealth;  //Start health of enemy

    //Private variables
    private float m_CurrentHealth;  //Current health of enemy
    private bool m_Dead;            //Enemy is dead or not

    //Gets called every time something hits the base and playerShell will be set inactive
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShell"))
        {
            other.gameObject.SetActive(false);
            TakeDamage(1f);
        }
		//if colide with base, damage base and set enemy to inactive
		if (other.gameObject.CompareTag ("Base")) {
			Debug.Log ("hoi");
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody> ();
			if (targetRigidbody) {
				Basehealth basehealth = targetRigidbody.GetComponent<Basehealth> ();
				basehealth.TakeDamage (1f);
				gameObject.SetActive (false);				
			}
		}
    }

    //Take damage
    public void TakeDamage(float amount)
    {
        if (amount > 0)
        {
            m_CurrentHealth -= amount;
        }

        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    // OnDeath
    private void OnDeath()
    {
        m_Dead = true;
        gameObject.SetActive(false);
    }
}
