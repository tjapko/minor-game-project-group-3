using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie3 : EnemyManager
{
	public GameObject enemyPrefab;

	//Chooses whether player or base is closest, walks there first, then to the other
	public Enemie3 (GameObject instance, Transform spawnpoint, GameObject _base, Transform playerpoint, int number) : base(instance, spawnpoint, _base, playerpoint, number)
	{
		this.m_SpawnPoint = spawnpoint;
		this.m_Base = _base;
		this.m_EnemyNumber = number;
		this.m_Instance = instance;
		this.m_PlayerPoint = playerpoint;

		this.health = m_Instance.GetComponent<EnemyHealth> ();

		m_Instance.AddComponent<UnitPlayer> ();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
		health.playerUnit = m_MovementPlayer;
		m_MovementPlayer.m_player = m_PlayerPoint;
		m_MovementPlayer.m_base = m_Base;
		m_MovementPlayer.speed = m_MovementPlayer.normalSpeed;
		m_MovementPlayer.calcDistance (true);
	}
}