using System;
using UnityEngine;

/// <summary>
/// Class enemy manager
/// </summary>
[Serializable]
public class EnemyManager
{
    //Public variables
    public Transform m_SpawnPoint;                      // Spawn position of enemy (should be appointed by Gamemanger instead of manual)
    [HideInInspector] public Transform m_TargetPoint;   // Location of target
    [HideInInspector] public int m_EnemyNumber;         // Number of enemy
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the enemy
    [HideInInspector] public EnemyMovement m_Movement;  // Reference to enemy's movement script, used to disable and enable control.
    //private GameObject m_CanvasGameObject;            // Used to disable the world space UI during the Starting and Ending phases of each round.

    //Setup
    public void Setup()
    {
        // Get references to the components.
        m_Movement = m_Instance.GetComponent<EnemyMovement>();
        m_Movement.m_targetlocation = m_TargetPoint;
        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
    }


    // Used during the phases of the game where the enemy shouldn't move
    public void DisableControl()
    {
        m_Movement.enabled = false;
    }


    // Used during the phases of the game where the enemy should be able to move
    public void EnableControl()
    {
        m_Movement.enabled = true;
    }


    // Used at the start of each round to put the enemy into the default state
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
    
}
