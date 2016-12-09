using System;
using System.Collections;
using UnityEngine;
using UnityEditor.VersionControl;

public class EnemyToPlayer : EnemyManager
{
	public GameObject enemyPrefab;
	public EnemyHealth health;
	public UnitPlayer playerUnit;

	public EnemyToPlayer (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_BasePoint = basetarget;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;
		this.health = m_Instance.GetComponent<EnemyHealth> ();
		this.playerUnit = m_Instance.GetComponent<UnitPlayer> ();
		playerUnit.enabled = false;
		health.m_playerPoint = m_PlayerPoint;
		m_Movement.m_target = m_BasePoint;
	}
}
