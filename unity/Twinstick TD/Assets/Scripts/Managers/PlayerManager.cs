using System;
using UnityEngine;


[Serializable]
public class PlayerManager
{
    /*
    public Color m_PlayerColor;     // Colour of player
    public Transform m_SpawnPoint;  // Spawn position of player
    [HideInInspector] public int m_PlayerNumber;            // Number of player
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the player

    //private PlayerMovement m_Movement;                        // Reference to player's movement script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    public void Setup()
    {
        // Get references to the components.
        m_Movement = m_Instance.GetComponent<PlayerMovement>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        // Set the player numbers to be consistent across the scripts.
        m_Movement.m_PlayerNumber = m_PlayerNumber;

        // Get all of the renderers of the tank and set to colour
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    // Used during the phases of the game where the player shouldn't be able to control their tank.
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    // Used during the phases of the game where the player should be able to control their tank.
    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    // Used at the start of each round to put the tank into it's default state.
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    */
}
