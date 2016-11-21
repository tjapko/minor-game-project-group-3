﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//UNDER CONSTRUCTION
/// <summary>
/// Class GameManager
/// </summary>
public class GameManager : MonoBehaviour
{
    //Public variables
    public float m_StartDelay = 3f;             // The delay between the start of round and playing of round
    public float m_waveDelay = 5f;              // The delay between ending and starting of wave
    public float m_EndDelay = 3f;               // The delay between losing and restarting
    public CameraControl m_CameraControl;     // Reference to the CameraControl script for control during different phases.
    public GameObject m_baseprefab;             // Reference to the base
    public GameObject m_Playerprefab;           // Reference to the prefab the players will control.
    public GameObject m_Enemyprefab;            // Reference to the prefab of the enemies.
    public BaseManager m_base;                  // The base manager of the base
    public PlayerManager[] m_players;           // A collection of managers for enabling and disabling different aspects of the player.
    public EnemyManager[] m_enemies;            // A collection of managers for enabling and disabling different aspects of the enemies.

    //Private variables
    private int m_waveNumber;                   // Which wave the game is currently on.
    private int m_enemy_number;                 // Used to track the enemy
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_waveWait;          // Time between waves (not yet used)
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.

    //Start
    private void Start()
    {
        Debug.ClearDeveloperConsole();
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        m_enemy_number = 0;
        m_waveNumber = 0;
        spawnbase();
        spawnAllPlayers();

        SetCameraTargets();

        // Once the players and base has been created start game
        StartCoroutine(GameLoop());
    }

    // Spawn the base
    private void spawnbase()
    {
        m_base.m_Instance = Instantiate(m_baseprefab, m_base.m_SpawnPoint.position, m_base.m_SpawnPoint.rotation) as GameObject;
    }

    
    // Spawn all the players
    private void spawnAllPlayers()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].m_Instance =
                Instantiate(m_Playerprefab, m_players[i].m_SpawnPoint.position, m_players[i].m_SpawnPoint.rotation) as GameObject;
            m_players[i].m_PlayerNumber = i + 1;
            m_players[i].Setup();
        }
    }
    
    // Spawn all the enemies
    private void SpawnEnemies(int m_number_enemies)
    {
        for (int i = 0; i < m_number_enemies; i++)
        {
            m_enemies[i].m_Instance =
                Instantiate(m_Enemyprefab, m_enemies[i].m_SpawnPoint.position, m_enemies[i].m_SpawnPoint.rotation) as GameObject;
            m_enemies[i].m_TargetPoint = m_base.m_SpawnPoint;
            m_enemies[i].m_EnemyNumber = i+1;
            m_enemies[i].Setup();
        }
    }
    

    
    //Sets position of camera based on players
    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[m_players.Length];
        
        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = m_players[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }
    

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        
        yield return StartCoroutine(Startgame());

        //Play round
        yield return StartCoroutine(RoundPlaying());
      
        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        //yield return StartCoroutine(RoundEnding());

        //Restart game
        StartCoroutine(GameLoop());
    }

    
    // Starting game
    private IEnumerator Startgame()
    {
        // As soon as the round starts reset the players and make sure they can't move.
        resetAllPlayers();
        enablePlayerControl();

        // Snap the camera's zoom and position to something appropriate for the reset tanks.
        //m_CameraControl.SetStartPositionAndSize();

        // Increment the round number and display text showing the players what round it is.
        m_waveNumber++;
        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }

    
    private IEnumerator RoundPlaying()
    {

        // Clear the text from the screen.
        // m_MessageText.text = string.Empty;
        SpawnEnemies(1);
        enableEnemyControl();
        // While there is not one tank left...
        while (!playerDead() || enemyDead())
        {
            // ... return on the next frame.
            yield return null;
        }

        StartCoroutine(RoundPlaying());

    }

    /*
    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        disablePlayerControl();

        yield return m_EndWait;
    }
    */
    
    // Determine if players are dead (hasn't been tested)
    private bool playerDead()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            if (m_players[i].m_Instance.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    // Determine if all the enemies are dead
    private bool enemyDead()
    {
        for (int i = 0; i < m_enemies.Length; i++)
        {
            if (m_enemies[i].m_Instance.activeSelf)
            {
                return false;
            }
        }

        return true;
    }
    
    // Reset player position(tested)
    private void resetAllPlayers()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].Reset();
        }
    }

    // Remove all enemies
    /*
    private void killallenemies()
    {
        for(int i = 0; i < m_enemies.Length; i++)
        {
            m_enemies[i].kill();
        }
    }
    */

    // Enable player control
    private void enablePlayerControl()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].EnableControl();
        }
    }
    
    //Disable player control
    private void disablePlayerControl()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].DisableControl();
        }
    }

    // Enable enemy control
    private void enableEnemyControl()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_enemies[i].EnableControl();
        }
    }

    //Disable enemy control
    private void disableEnemyControl()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_enemies[i].DisableControl();
        }
    }
}