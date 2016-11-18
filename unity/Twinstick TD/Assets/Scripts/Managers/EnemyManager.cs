﻿using System;
using UnityEngine;


[Serializable]
public class EnemyManager
{
    public Transform m_SpawnPoint;                          // Spawn position of enemy
    [HideInInspector] public int m_EnemyNumber;            // Number of enemy
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the enemy

    private EnemyMovement m_Movement;                       // Reference to enemy's movement script, used to disable and enable control.
	private int m_Health;
	private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    public void Setup()
    {
        // Get references to the components.
        m_Movement = m_Instance.GetComponent<EnemyMovement>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        // Set the enemy number to be consistent across the scripts.
//        m_Movement.m_PlayerNumber = m_PlayerNumber;
    }


    // Used during the phases of the game where the enemy shouldn't move
    public void DisableControl()
    {
        m_Movement.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    // Used during the phases of the game where the enemy should be able to move
    public void EnableControl()
    {
        m_Movement.enabled = true;

        m_CanvasGameObject.SetActive(true);
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
