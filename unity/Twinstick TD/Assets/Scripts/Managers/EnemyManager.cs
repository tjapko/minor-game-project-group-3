using System;
using UnityEngine;

/// <summary>
/// Class enemy manager
/// </summary>
//[Serializable]
public abstract class EnemyManager
{
    //Public variables
    public Transform m_SpawnPoint;                      // Spawn position of enemy (should be appointed by Gamemanger instead of manual)
    [HideInInspector] public Transform m_BasePoint;     // Location of base
	[HideInInspector] public Transform m_PlayerPoint; 	// Location of player
    [HideInInspector] public int m_EnemyNumber;         // Number of enemy
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the enemy
    [HideInInspector] public UnitPlayer m_MovementPlayer;  	// Reference to enemy's movement script, used to disable and enable control.
	[HideInInspector] public EnemyHealth health;		//Enemie health script


    //Constructor
	public EnemyManager(GameObject instance, Transform spawnpoint, Transform basetarget, Transform playertarget, int number)
    {
        m_SpawnPoint = spawnpoint;
        m_BasePoint = basetarget;
        m_EnemyNumber = number;
        m_Instance = instance;
		this.health = m_Instance.GetComponent<EnemyHealth> ();
    }


	// Used during the phases of the game where the enemy shouldn't move
	public void EnableControl()
	{
		{
			m_Instance.GetComponent<UnitPlayer> ().enabled = true;
		}
	}

	// Used during the phases of the game where the enemy should be able to move
	public void DisableControl()
	{
		m_Instance.GetComponent<UnitPlayer> ().enabled = false;
	}

    // Used at the start of each round to put the enemy into the default state
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

}
