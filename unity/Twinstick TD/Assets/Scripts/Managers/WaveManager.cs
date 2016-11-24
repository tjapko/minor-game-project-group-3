using UnityEngine;
using System.Collections.Generic;

public class WaveManager
{
    //Public variables
    public GameObject m_Enemyprefab;       //Reference to prefab of enemy
    public Transform m_enemyspawnpoints;   //Spawnpoints of enemies
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
        SpawnEnemies(1);
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

    // Spawn enemies
    private void SpawnEnemies(int m_number_enemies)
    {
        for (int i = 0; i < m_number_enemies; i++)
        {
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
