﻿using UnityEngine;
using System.Collections.Generic;
//using System;


public class WaveManager
{
    //Public variables
    public GameObject m_Enemyprefab1;       //Reference to prefab of enemy
	public GameObject m_EnemyprefabToPlayer;


	public GameObject m_gridprefab;
	[HideInInspector]public Transform m_enemyspawnpoints;   //Spawnpoints of enemies
    public Transform m_basetarget;             //Target(s) of enemies
	public Transform m_playerpoint;
    public double baseDistancePercentage = 0.1;
    Grid grid;
    public LayerMask unwalkableMask;
    public float nodeRadius;

    //Private variables
    private List<EnemyManager> m_enemywave; //Population of enemies
    private int enemy_number;               //Total amount of enemies spawned
	private int m_wavenumber;				//Total amount of waves
	private int numberEnemiesPerWave;		//Start amount of enemies per wave
	private GridManager m_gridmanager;

    // Use this for initialization
	public WaveManager(GameObject Enemyprefab, GameObject EnemyToPlayer, Transform enemyspawnpoints, Transform basetarget, Transform playerpoint, GameObject gridprefab)
    {
        this.m_Enemyprefab1 = Enemyprefab;
		this.m_EnemyprefabToPlayer = EnemyToPlayer;
        this.m_enemyspawnpoints = enemyspawnpoints;
        this.m_basetarget = basetarget;
		this.m_playerpoint = playerpoint;
		this.m_gridprefab = gridprefab;

		this.m_wavenumber = 0;
		this.numberEnemiesPerWave = 10;
        enemy_number = 0;
        m_enemywave = new List<EnemyManager>();
    }

    // Send next wave and create new grid
    public void NextWave()
	{
		if (m_wavenumber == 0) {
			m_gridmanager = new GridManager (m_gridprefab);
			grid = GameObject.FindWithTag ("grid").GetComponent<Grid> ();
		}
		if (m_wavenumber > 0) {
			GameObject.Destroy (m_gridmanager.m_instance, 0f);
			m_gridmanager = new GridManager (m_gridprefab);
			grid = GameObject.FindWithTag ("grid").GetComponent<Grid> ();
		}
		int enemies = numberEnemiesPerWave + EnemiesAmountPerWave ();
		SpawnEnemies(enemies);
		m_wavenumber++;
    }

	// Function for amount of enemies next wave
	public int EnemiesAmountPerWave(){
		return m_wavenumber*2;
	}

    //Check if all enemies are dead;
    public bool EnemiesDead()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            if (enemy.m_Instance.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

	private Vector3 RandomSpawnPosition() 
	{
		Vector3 randomPosition;

		float buffer = 1.0f;  	// buffer for extra space between enemies and wall maybe not needed for later (walkable will fix this)
		bool walkable = true;	// needs to be implmented! 
		float distance;		 	// distance between base and enemies spawnpoint 

		// base's spawning position
		Vector3 Base = GameObject.FindGameObjectWithTag ("Base").GetComponent<Transform> ().transform.position;

		// floats for holding dimensions of the map (walls)
		float x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
		float x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
		float z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
		float z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;

		// enemies needs to be spawned at least 50% of the x-dimenion of the base
		float crit_distance  = (float)baseDistancePercentage * (x_maxrange - x_minrange); 

		do {
			randomPosition = new Vector3 (Random.Range (x_minrange, x_maxrange), 0f, Random.Range (z_minrange, z_maxrange));

            walkable = !(Physics.CheckSphere(randomPosition, (grid.nodeRadius * 1.4f), grid.unwalkableMask));
            distance = Vector3.Distance(Base, randomPosition);
		} while (distance <= crit_distance || !walkable); // distance needs to be smaller than critical distance and the spawnpoint needs to be walkable
		return randomPosition;
	}
    // Spawn enemies
    private void SpawnEnemies(int m_number_enemies)
    {
        for (int i = 0; i < m_number_enemies; i++)
        {
			m_enemyspawnpoints.position = RandomSpawnPosition ();
            GameObject newinstance = GameObject.Instantiate(m_EnemyprefabToPlayer, m_enemyspawnpoints.position, m_enemyspawnpoints.rotation) as GameObject;
			m_enemywave.Add(new EnemyToPlayer(newinstance, m_enemyspawnpoints, m_basetarget, m_playerpoint, enemy_number));
			enemy_number++;
        }

    }

    //Remove dead enemies
    public void DestroyEnemies()
    {
        int n = m_enemywave.Count;
        for (int i = 0; i < n; i++)
        {
            GameObject.Destroy(m_enemywave[0].m_Instance);
            m_enemywave.RemoveAt(0);
        }

    }

    //Enable control of enemies
    public void EnableEnemyWaveControl()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            enemy.EnableControl();
        }
    }

    //Disable control of enemies
    public void DisableEnemyWaveControl()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            enemy.DisableControl();
        }
    }

}
