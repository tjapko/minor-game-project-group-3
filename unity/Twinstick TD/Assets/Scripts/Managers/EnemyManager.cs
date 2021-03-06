﻿using System;
using UnityEngine;

/// <summary>
/// Class enemy manager
/// </summary>
//[Serializable]
public abstract class EnemyManager
{
    //Public variables
    public Transform m_SpawnPoint;                      // Spawn position of enemy (should be appointed by Gamemanger instead of manual)
	[HideInInspector] public GameObject m_Base;     // Location of base
	[HideInInspector] public Transform m_PlayerPoint; 	// Location of player
    [HideInInspector] public int m_EnemyNumber;         // Number of enemy
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the enemy
	[HideInInspector] public EnemyHealth health;		//Enemie health script
	[HideInInspector] public UnitPlayer m_MovementPlayer;
	private EnemyInheratedValues inheratedValues;
    private float m_damageDoneToObject = 0f;
    private float m_damageDoneToPlayer = 0f;


    

    //Constructor

    public EnemyManager(GameObject instance, Transform spawnpoint, GameObject _base, Transform playertarget, int number, EnemyInheratedValues enemyInheratedValues)

    {
        m_SpawnPoint = spawnpoint;
        m_Base = _base;
        m_EnemyNumber = number;
        m_Instance = instance;
        m_PlayerPoint = playertarget;
        this.inheratedValues = enemyInheratedValues;
        this.health = m_Instance.GetComponent<EnemyHealth> ();
        this.health.setDamageToTowerSec(inheratedValues.getDamageToObjectPerAttack());
        this.health.setTowerperSecond(inheratedValues.getAttackSpeedObject());
        this.health.setDamageToPlayerSec(inheratedValues.getDamgeToPlayerPerAttack());
        this.health.setPlayerPerSecond(inheratedValues.GetAttackSpeedPlayer());
        this.health.setCurrentHealth(inheratedValues.getStartingHealth());
		m_Instance.AddComponent<UnitPlayer>();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer>();
		m_MovementPlayer.setMovementspeed (inheratedValues.getMovementspeed ());
    }


	// Used during the phases of the game where the enemy shouldn't move
	public void EnableControl()
	{
		m_Instance.GetComponent<UnitPlayer> ().enabled = true;
    }

	// Used during the phases of the game where the enemy should be able to move
	public void DisableControl()
	{
		m_Instance.GetComponent<UnitPlayer> ().enabled = false;

	}

    // Used at the start of each round to put the enemy into the default state
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    public void updatePreformance()
    {
        this.m_damageDoneToObject = this.health.getDamageDoneToObject();
        this.m_damageDoneToPlayer = this.health.getDamageDoneToPlayer();
        this.inheratedValues.setDamageDone(this.m_damageDoneToObject, this.m_damageDoneToPlayer);
    }

}
