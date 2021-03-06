﻿using UnityEngine;
using System;

/// <summary>
/// Class BaseManager 
/// </summary>
[Serializable]
public class BaseManager
{
    //public variables
    public Transform m_SpawnPoint;                      // Spawn position of base
    public GameObject m_baseprefab;                     // Reference to prefab of base
    public GameObject m_Instance;                       // Reference to instance of base

    //private variables
    public Basehealth m_basehealth;                    //Reference to base health script

    //Constructor
    public BaseManager (GameObject baseprefab, Transform spawnpoint)
    {
        this.m_baseprefab = baseprefab;
        this.m_SpawnPoint = spawnpoint;
    }

    //Spawn base
    public void spawnBase()
    {
		if (m_Instance != null) {
			m_Instance.GetComponent<Basehealth> ().OnEnable();
		} else {
        	m_Instance = GameObject.Instantiate(m_baseprefab, m_SpawnPoint.position, m_SpawnPoint.rotation) as GameObject;
        	m_SpawnPoint = m_Instance.transform;
        	m_basehealth = m_Instance.GetComponent<Basehealth>();
		}

        m_Instance.GetComponent<BaseUpgradeScript>().StartInitialization();
    }

    //Reset function
    public void Reset()
    {
        //Reset base position and direction
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        //Reset active value
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
