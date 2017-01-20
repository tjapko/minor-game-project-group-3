using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie2 : EnemyManager
{
	public GameObject enemyPrefab;

    public Enemie2(GameObject instance, Transform spawnpoint, GameObject _base, Transform playerpoint, int number, EnemyInheratedValues enemyInheratedValues) : base(instance, spawnpoint, _base, playerpoint, number, enemyInheratedValues)
	{


        //movement
        health.playerUnit = m_MovementPlayer;
        m_MovementPlayer.m_player = m_PlayerPoint;
        m_MovementPlayer.m_base = m_Base;
		m_MovementPlayer.goToPlayer();
    }
}
