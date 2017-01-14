using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie1 : EnemyManager
{
	public GameObject enemyPrefab;

	//Walks towards base first, after hit to player
	public Enemie1 (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number, EnemyInheratedValues enemyInheratedValues) : base(instance, spawnpoint, basetarget, playerpoint, number, enemyInheratedValues)
	{
	
        //movement
		m_Instance.AddComponent<UnitPlayer> ();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
		health.playerUnit = m_MovementPlayer;
		m_MovementPlayer.m_player = m_PlayerPoint;
		m_MovementPlayer.m_base = m_BasePoint;
		m_MovementPlayer.goToBase ();
	}

   
}