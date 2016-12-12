using System;
using System.Collections;
using UnityEngine;
using UnityEditor.VersionControl;

public class EnemyToPlayer : EnemyManager
{
	public GameObject enemyPrefab;

	public EnemyToPlayer (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_BasePoint = basetarget;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;
		this.health = m_Instance.GetComponent<EnemyHealth> ();
		m_Instance.AddComponent<UnitPlayer> ();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
		m_MovementPlayer.m_player = m_PlayerPoint;
		m_MovementPlayer.StartIn ();
	}

	public override void movementSwitch(){
	}
}
