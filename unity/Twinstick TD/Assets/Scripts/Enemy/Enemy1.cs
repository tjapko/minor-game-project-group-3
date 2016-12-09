/*using System;
using System.Collections;
using UnityEngine;
using UnityEditor.VersionControl;

public class Enemy1 : EnemyManager
{
	public GameObject enemy1Prefab;
	public EnemyHealth health;
	public UnitPlayer playerUnit;

	public Enemy1 (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_BasePoint = basetarget;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;
		this.m_Movement = m_Instance.GetComponent<Unit> ();
		this.health = m_Instance.GetComponent<EnemyHealth> ();
		this.playerUnit = m_Instance.GetComponent<UnitPlayer> ();
		playerUnit.enabled = false;
		m_Movement.m_target = m_BasePoint;
	}

}
*/