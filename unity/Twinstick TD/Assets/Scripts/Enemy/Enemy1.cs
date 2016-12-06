using System;
using UnityEngine;

public class Enemy1 : EnemyManager
{
	public EnemyHealth health;

	public Enemy1 (GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number) : base(instance, spawnpoint, basetarget, playerpoint, number)
	{
		m_SpawnPoint = spawnpoint;
		m_BasePoint = basetarget;
		m_EnemyNumber = number;
		m_Instance = instance;
		m_Movement = m_Instance.GetComponent<Unit> ();
		health = m_Instance.GetComponent<EnemyHealth> ();
	}


	public void moveFunction(){
		m_Movement.m_target = m_BasePoint;
		if (health.basehit) {
			m_Movement.m_target = m_PlayerPoint;
		}
	}
}
