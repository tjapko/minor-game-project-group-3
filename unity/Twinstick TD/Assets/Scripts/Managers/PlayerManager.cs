using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerManager
/// </summary>
[Serializable]
public class PlayerManager
{
    // Public variables
    public GameObject m_Instance;       // A reference to the instance of the player (Instantiated by gamer manager)
    public Transform m_SpawnPoint;      // Spawn position of player

    //References
    public PlayerMovement m_movement;   // Reference to player's movement script
    public BulletFire m_shooting;       // Reference to player's shooting script
    public PlayerStatistics m_stats;    // Reference to player's statisitics script
    public PlayerConstruction m_construction; // Reference to player's construction script
    public PlayerInventory m_inventory;	// Reference to player's inventory script
	public PlayerHealth m_playerhealth;	// Reference to player's health script

    //Private variables
    public int m_PlayerNumber;          // Number of player

    //Constructor
    public PlayerManager (Transform spawnpoint, int playernumber, GameObject instance)
    {
        m_Instance = instance;
        m_SpawnPoint = spawnpoint;
        m_PlayerNumber = playernumber;
        
		//Set references
		m_playerhealth = m_Instance.GetComponent<PlayerHealth>();
        m_movement = m_Instance.GetComponent<PlayerMovement>();
        m_shooting = m_Instance.GetComponent<BulletFire>();
        m_stats = m_Instance.GetComponent<PlayerStatistics>();
        m_construction = m_Instance.GetComponent<PlayerConstruction>();
        m_inventory = m_Instance.GetComponent<PlayerInventory>();

        //Set variables
        m_stats.m_playernumber = playernumber;

    }

    // Enable control of player
    public void EnableControl()
    {
        m_movement.enabled = true;
        m_shooting.enabled = true;
        m_stats.enabled = true;
        m_construction.enabled = true;
        m_inventory.enabled = true;
    }

    // Disable control of player
    public void DisableControl()
    {
        m_movement.enabled = false;
        m_shooting.enabled = false;
        m_stats.enabled = false;
        m_construction.enabled = false;
        m_inventory.enabled = false;
    }

    // Reset state of player
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;
        m_construction.removePlayerConstructions();

		m_playerhealth.Start();

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    
}
