﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Script CarrotFieldScript
/// Contains the basic functions for the carrot field
/// </summary>
public class CarrotFieldScript : MonoBehaviour {
	[Header("References")]
	public GameObject m_hitBaseCanvasPrefab;    //Reference to friendlyHit canvas;
	public Slider m_Slider;
	public Image m_FillImage;

	[Header("Public variables")]
    //Public Variables
    public float m_startyield;                //Yield of carrot field at start
    public float m_inc;                       //Yield(n+1) = inc * Yield(n)
	public float m_startHealth;
	public Color m_FullHealthColor = Color.green;   //Full health colour
	public Color m_ZeroHealthColor = Color.red;     //Zero health colour

	//Private variables
    private float m_currentYield;           //Current yield Y(n)
	private float m_currentHealth;
	private bool m_Dead;

    void Start()
    {
		m_Dead = false;
        m_currentYield = m_startyield;
		m_currentHealth = m_startHealth;
		SetHealthUI ();
    }

	public void takeDamage(float damage){
		//Set hitmark
		createHitMark(m_hitBaseCanvasPrefab, damage);

		// Reduce current health by the amount of damage done.
		m_currentHealth -= damage;
		// Change the UI elements appropriately.
		SetHealthUI();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (m_currentHealth <= 0f && !m_Dead)
		{
			CancelInvoke ();
			OnDeath();
		}
	}

	private void SetHealthUI()
	{
		// Set the slider's value appropriately.
		m_Slider.value = m_currentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_currentHealth / m_startHealth);
	}

	//Spawn hitmark
	private void createHitMark(GameObject prefab, float amount)
	{
		//Set hitmark
		GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		hitbox.GetComponent<HitMarkScript>().setDamage(amount);
	}

	// OnDeath
	private void OnDeath()
	{
		m_Dead = true;
		gameObject.SetActive(false);
	}

    //Initialize variables
    public int waveYield()
    {
        
        //m_currentYield = m_currentYield * m_inc;
		return (int)m_startyield;
    }

}
