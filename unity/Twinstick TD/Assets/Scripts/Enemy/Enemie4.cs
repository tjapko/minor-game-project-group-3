using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie4 : EnemyManager
{
	public GameObject enemyPrefab;

    public Enemie4(GameObject instance, Transform spawnpoint, GameObject _base, Transform playerpoint, int number, EnemyInheratedValues enemyInheratedValues) : base(instance, spawnpoint, _base, playerpoint, number, enemyInheratedValues)
	{


        //movement
        health.playerUnit = m_MovementPlayer;
        m_MovementPlayer.m_player = m_PlayerPoint;
        m_MovementPlayer.m_base = m_Base;
		m_MovementPlayer.movementSpeed = m_MovementPlayer.bossSpeed;
        m_MovementPlayer.calcDistance(true);
		m_MovementPlayer.m_scalePush = 0.5f;
    }
}