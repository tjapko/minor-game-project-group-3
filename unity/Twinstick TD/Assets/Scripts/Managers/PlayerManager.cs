using System;
using UnityEngine;

/// <summary>
/// PlayerManager
/// </summary>
[Serializable]
public class PlayerManager
{
    //public Color m_PlayerColor;       // Colour of player
    public Transform m_SpawnPoint;      // Spawn position of player
    [HideInInspector] public int m_PlayerNumber;        // Number of player
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the player (Instantiated by gamer manager)
    [HideInInspector] public PlayerMovement m_movement; // Reference to player's movement script
    [HideInInspector] public BulletFire m_shooting;     // Reference to player's shooting script

    //Constructor
    public PlayerManager (Transform spawnpoint, int playernumber, GameObject instance)
    {
        m_SpawnPoint = spawnpoint;
        m_PlayerNumber = playernumber;
        m_Instance = instance;

        Setup();
    }

    //Setup
    public void Setup()
    {
        // Get references to the components.
        m_movement = m_Instance.GetComponent<PlayerMovement>();
        m_shooting = m_Instance.GetComponent<BulletFire>();
        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

    }

    // Enable control of player
    public void EnableControl()
    {
        m_movement.enabled = true;
        m_shooting.enabled = true;
        //m_CanvasGameObject.SetActive(true);
    }

    // Disable control of player
    public void DisableControl()
    {
        m_movement.enabled = false;
        m_shooting.enabled = false;
        //m_CanvasGameObject.SetActive(false);
    } 

    // Reset state of player
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    
}
