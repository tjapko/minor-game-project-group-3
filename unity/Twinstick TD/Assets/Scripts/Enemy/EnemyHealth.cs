using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class EnemyHealth
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    //Public variables
    public float m_StartingHealth;  //Start health of enemy
    public Slider m_Slider;                             // The slider to represent how much health the enemy currently has.
    public Image m_FillImage;                           // The image component of the slider.
    public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
    public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
    //Private variables
    private float m_CurrentHealth;  //Current health of enemy
    private bool m_Dead;            //Enemy is dead or not


    private void OnEnable()
    {
        // When the enemy is enabled, reset the enemy's health
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        // Update the health slider's value and color.
        SetHealthUI();
    }

    //Gets called every time something hits the base and playerShell will be set inactive
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerShell"))
        {
            other.gameObject.SetActive(false);
            TakeDamage(1f);
        }
        //if colide with base, damage base and set enemy to inactive
        if (other.gameObject.CompareTag("Base"))
        {
            Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();
            if (targetRigidbody)
            {
                Basehealth basehealth = targetRigidbody.GetComponent<Basehealth>();
                basehealth.TakeDamage(1f);
                gameObject.SetActive(false);
            }
        }
    }

    //Take damage
    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done.
        m_CurrentHealth -= amount;

        // Change the UI elements appropriately.
        SetHealthUI();

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
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