﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public float m_StartingHealth;	//Start health of enemy

	[HideInInspector]public Basehealth tower;

	private float m_CurrentHealth;	//Current health of enemy
	private bool m_Dead;			//Enemy is dead or not


	public void SetUp(){
		
	}

	//gets called every time something hits the base and playerShell will be set inactive
	public void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("PlayerShell")) {
			other.gameObject.SetActive (false);
			TakeDamage (1f);
		}

			if (other.gameObject.CompareTag ("Base")) {
				Rigidbody targetRigidbody = other.GetComponent<Rigidbody> ();
				if (targetRigidbody) {
				Basehealth basemanager = targetRigidbody.GetComponent<Basehealth> ();
				basemanager.TakeDamage (1f);
				gameObject.SetActive (false);				
			}
		}

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

	// OnDeath
	private void OnDeath()
	{
		// Play the effects for the death of the tank and deactivate it.
		m_Dead = true;
		gameObject.SetActive(false);
	}
}
