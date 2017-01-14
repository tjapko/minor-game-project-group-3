using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie4 : EnemyManager
{
	public GameObject enemyPrefab;

    //Boss enemie, walks slowly towards base
    public Enemie4(GameObject instance, Transform spawnpoint, Transform basetarget, Transform playerpoint, int number, EnemyInheratedValues enemyInheratedValues) : base(instance, spawnpoint, basetarget, playerpoint, number, enemyInheratedValues)
	{

        //movement
        m_Instance.AddComponent<UnitPlayer> ();
		this.m_MovementPlayer = m_Instance.GetComponent<UnitPlayer> ();
		health.playerUnit = m_MovementPlayer;
		m_MovementPlayer.m_player = m_PlayerPoint;
		m_MovementPlayer.m_base = m_BasePoint;
		m_MovementPlayer.playerFirst = true;
		m_MovementPlayer.speed = 3.0f;
		m_MovementPlayer.goToBase();
	}
}