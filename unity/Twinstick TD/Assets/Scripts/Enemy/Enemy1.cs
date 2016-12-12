using System;
using System.Collections;
using UnityEngine;
using UnityEditor.VersionControl;

public class Enemy1 : EnemyManager
{
	public GameObject enemy1Prefab;
	public UnitPlayer playerUnit;

	public Enemy1 (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_BasePoint = basetarget;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;
		m_Instance.AddComponent<Unit> ();
		this.m_MovementUnit = m_Instance.GetComponent<Unit> ();
		this.health = m_Instance.GetComponent<EnemyHealth> ();
		m_MovementUnit.m_target = m_BasePoint;
	}

	public override void movementSwitch(){
		//delete component Unit
		m_Instance.SetActive(false);
		m_Instance.AddComponent<UnitPlayer>();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
	}
}
