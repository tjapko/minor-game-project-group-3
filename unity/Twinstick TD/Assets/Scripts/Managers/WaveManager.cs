using UnityEngine;
using System.Collections.Generic;

public class WaveManager
{
    //Public variables
    public GameObject m_Enemyprefab;       //Reference to prefab of enemy
	[HideInInspector]public Transform m_enemyspawnpoints;   //Spawnpoints of enemies
    public Transform m_target;             //Target(s) of enemies

    //Private variables
    private List<EnemyManager> m_enemywave;       //Population of enemies
    private int enemy_number;               //Total amount of enemies spawned

    // Use this for initialization
    public WaveManager(GameObject Enemyprefab, Transform enemyspawnpoints, Transform target)
    {
        this.m_Enemyprefab = Enemyprefab;
        this.m_enemyspawnpoints = enemyspawnpoints;
        this.m_target = target;

        enemy_number = 0;
        m_enemywave = new List<EnemyManager>();
    }

    // Send next wave
    public void NextWave()
    {
        SpawnEnemies(2);
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

	private Vector3 RandomPosition() 
	{
		float buffer = 1.0f;
		bool walkable = true; // needs to be implmented! 
		float x_minrange, x_maxrange, z_minrange, z_maxrange;
		float distance;
		Vector3 Base = GameObject.FindGameObjectWithTag ("Base").GetComponent<Transform> ().transform.position;

		Vector3 randomPosition;
		do {
			x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
			x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
			z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
			z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;
			randomPosition = new Vector3 (Random.Range (-x_minrange, x_maxrange), 0f, Random.Range (z_minrange, z_maxrange));

			distance = Vector3.Distance(Base, randomPosition);
		} while (distance <= 0.5 * (x_maxrange - x_minrange) && !walkable);
		return randomPosition;
	}

    // Spawn enemies
    private void SpawnEnemies(int m_number_enemies)
    {
        for (int i = 0; i < m_number_enemies; i++)
        {
			m_enemyspawnpoints.position = RandomPosition ();
            GameObject newinstance = GameObject.Instantiate(m_Enemyprefab, m_enemyspawnpoints.position, m_enemyspawnpoints.rotation) as GameObject;
            m_enemywave.Add(new EnemyManager(newinstance, m_enemyspawnpoints, m_target, enemy_number));
            enemy_number++;
        }
    }

    //Remove dead enemies
    public void DestroyEnemies()
    {
        for (int i = 0; i < m_enemywave.Count; i++)
        {
            if (!(m_enemywave[i].m_Instance.activeSelf))
            {
                GameObject.Destroy(m_enemywave[i].m_Instance);
                m_enemywave.RemoveAt(i);
            }
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
