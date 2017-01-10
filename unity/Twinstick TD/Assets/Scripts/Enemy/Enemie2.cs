using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie2 : EnemyManager
{
	public GameObject enemyPrefab;

	//Walks towards player first, untill distance is less than 8, then towards base
	public Enemie2 (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		

        // movement
		m_Instance.AddComponent<UnitPlayer> ();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
		health.playerUnit = m_MovementPlayer;
		m_MovementPlayer.m_player = m_PlayerPoint;
		m_MovementPlayer.m_base = m_BasePoint;
		m_MovementPlayer.goToPlayer ();
	}
}
