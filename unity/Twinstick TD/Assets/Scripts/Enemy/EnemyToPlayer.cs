using System;
using System.Collections;
using UnityEngine;
using UnityEditor.VersionControl;

public class EnemyToPlayer : EnemyManager
{
	public GameObject enemyPrefab;
	public EnemyHealth health;

	public EnemyToPlayer (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_BasePoint = basetarget;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;
		this.health = m_Instance.GetComponent<EnemyHealth> ();
		this.m_Movement = m_Instance.GetComponent<UnitPlayer> ();
		m_Movement.m_player = m_PlayerPoint;
		m_Movement.StartIn ();
	}
}
