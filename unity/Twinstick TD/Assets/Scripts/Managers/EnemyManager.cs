﻿using System;
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
    [HideInInspector] public Unit m_Movement;  // Reference to enemy's movement script, used to disable and enable control.

    //Constructor
    public EnemyManager(GameObject instance, Transform spawnpoint, Transform target, int number)
    {
        m_SpawnPoint = spawnpoint;
        m_TargetPoint = target;
        m_EnemyNumber = number;
        m_Instance = instance;
        m_Movement = m_Instance.GetComponent<Unit>();
        m_Movement.m_target = m_TargetPoint;
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
