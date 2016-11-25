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

    //private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

    //Setup
    public void Setup()
    {
        // Get references to the components.
        m_movement = m_Instance.GetComponent<PlayerMovement>();
        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

    }

    // Disable control of player
    public void DisableControl()
    {
        if(m_movement == null)
        {
            Debug.Log("ISNULL");
        }
        m_movement.enabled = false;

        //m_CanvasGameObject.SetActive(false);
    }

    // Enable control of player
    public void EnableControl()
    {
        m_movement.enabled = true;

        //m_CanvasGameObject.SetActive(true);
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
