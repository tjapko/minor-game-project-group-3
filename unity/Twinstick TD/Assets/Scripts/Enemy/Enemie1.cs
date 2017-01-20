using System;
using System.Collections;
using UnityEngine;
//using UnityEditor.VersionControl;

public class Enemie1 : EnemyManager
{
	public GameObject enemyPrefab;

    //Walks towards base first, after hit to player
    public Enemie1(GameObject instance, Transform spawnpoint, GameObject _base, Transform playerpoint, int number, EnemyInheratedValues enemyInheratedValues) : base(instance, spawnpoint, _base, playerpoint, number, enemyInheratedValues)
	{


        //movement

        health.playerUnit = m_MovementPlayer;
        m_MovementPlayer.m_player = m_PlayerPoint;
        m_MovementPlayer.m_base = m_Base;
        m_MovementPlayer.calcDistance(false);
    }


}