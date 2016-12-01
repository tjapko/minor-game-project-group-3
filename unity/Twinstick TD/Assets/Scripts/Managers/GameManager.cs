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
    public int m_amountofplayers;               // Total amount of players that are participating
    public float m_StartDelay = 3f;             // The delay between the start of round and playing of round
    public float m_waveDelay = 15f;              // The delay between ending and starting of wave
    public float m_EndDelay = 3f;               // The delay between losing and restarting
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public GameObject m_uiprefab;               // Reference to UI prefab
    public GameObject m_baseprefab;             // Reference to the base
    public GameObject m_Playerprefab;           // Reference to the prefab the players will control.
    public GameObject m_Enemyprefab;            // Reference to the prefab of the enemies.
    public GameObject m_turret;                 // Reference to the turret prefab
    public Transform m_Basespawnpoint;          // Spawnpoint of base
    public Transform m_Playerspawnpoint;        // Spawnpoint of player
    [HideInInspector]public Transform m_Enemyspawnpoint;         // Spawnpoint of enemy


    //Private variables
    private MapUIScript m_uiscript;             // The UI script
    private BaseManager m_base;                 // The base manager of the base
    private UserManager m_players;              // A collection of managers for enabling and disabling different aspects of the players.
    private WaveManager m_wave;                 // A collection of managers for enabling and disabling different aspects of the enemies.
    private ObjectplacementManager m_objects;   // A collection of managers for enabling and disabling different aspects of the placed objects.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_waveWait;          // Time between waves (not yet used)
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private int m_waveNumber;                   // Which wave the game is currently on.
    bool gamepause;                             // Boolean if game is paused
    bool wavephase;                             // Boolean if game is in wavephase or construction phase 

    //Start
    private void Start()
    {
        //Setting up variables
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_waveWait = new WaitForSeconds(m_waveDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_waveNumber = 0;
        gamepause = false;
        wavephase = false;

        //Initialize managers
        m_wave = new WaveManager(m_Enemyprefab, m_Enemyspawnpoint, m_Basespawnpoint);
        m_players = new UserManager(m_Playerprefab, m_Playerspawnpoint, m_amountofplayers);
        m_base = new BaseManager(m_baseprefab, m_Basespawnpoint);
        m_objects = new ObjectplacementManager(m_players, m_turret);

        //Initialize UI script
        m_uiscript = new MapUIScript(gameObject.GetComponent<GameManager>(), m_uiprefab, m_players);

        //Spawning base and users
        m_players.spawnPlayers();
        m_base.spawnBase();

        //Set camera
        SetCameraTargets();

        // Start the game
        StartCoroutine(GameLoop());
    }

    //Check per frame
    private void Update()
    {
        //Escape key: pause menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gamepause = !gamepause;
            pauseGame(gamepause);
            
        }
        
        // Mouse click
        if (Input.GetKeyDown("1"))
        {
            //For final game : restore back to onmouse down and remove the coroutine
            //Set player click, and start coroutine object placement
            m_objects.setplayerclick(true);
            if(!wavephase)
            {
                StartCoroutine(m_objects.ObjectPlacement());
            }
        }

        // Mouse click
        if (Input.GetKeyUp("1"))
        {
            m_objects.setplayerclick(false);
        }

        //Set the Object placement phase
        m_objects.setconstructionphase(!wavephase);

        //Show or hide UI menu depending on wavephase and pause
        m_uiscript.UIchange(wavephase, gamepause);

        //Update score
        m_players.Update();
        m_uiscript.Update();

    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.

        yield return StartCoroutine(Startgame());

        //Play round
        yield return StartCoroutine(wavePhase());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

    }

    // Starting game
    private IEnumerator Startgame()
    {
        // Reset all players and enable control
        m_players.resetAllPlayers();
        m_players.enablePlayersControl();
        wavephase = false;

        m_CameraControl.SetStartPositionAndSize();

        // Wait m_StartWait of seconds before starting rounds
        yield return m_StartWait;
    }

    //Play round
    private IEnumerator wavePhase()
    {
        // Clear the text from the screen.
        // m_MessageText.text = string.Empty;
        
        // Wait until base has no health or players are dead
        while (!(m_players.playerDead() || m_base.BaseDead()))
        {
            wavephase = true;

            //Enemies are dead
            if (m_wave.EnemiesDead())
            {
                // Go into construction phase
                yield return StartCoroutine(constructionPhase());

                //Spawn next wave and remove dead enemies
                //While loop is needed, because EnemiesDead() is not fast enough to detect that a new wave has spawned
                m_wave.DestroyEnemies();
                while (m_wave.EnemiesDead())
                {
                    m_wave.NextWave();
                }
                m_waveNumber++;
                Debug.Log("Current wave" + m_waveNumber);
            }

            // Return next frame without delay
            yield return null;
        }

        //Player has lost
        Debug.Log("GAME OVER");
        
    }

    // Construction phase
    private IEnumerator constructionPhase()
    {
        //Set wavephase to false and set timer
        wavephase = false;
        StartCoroutine(constructionphaseTimer());
        
        //While the game is in construction phase perform actions
        //Can get out of while loop by getting signal from next wave button
        //Can get out of while loop when timer reaches time 
        // Timer must be shorter than time of ending next wave, incase user presses next wave button !!!! (needs fix)
        while(!wavephase)
        {
            yield return null;
        }

        //Starting wave phase
    }
    
    // Constuction phase countdown
    private IEnumerator constructionphaseTimer()
    {
        yield return m_waveWait;
        wavephase = true;
    }

    //Pause game function
    private void pauseGame(bool status)
    {
        if(status)
        {
            Time.timeScale = 0;
            m_players.disablePlayersControl();
            m_wave.DisableEnemyWaveControl();
        } else
        {
            Time.timeScale = 1;
            m_players.enablePlayersControl();
            m_wave.EnableEnemyWaveControl();
        }
        
    }

    
    private IEnumerator RoundEnding()
    {
        // Stop players and waves from moving.
        m_players.disablePlayersControl();
        m_wave.DisableEnemyWaveControl();

        yield return m_EndWait;
    }

    //Sets position of camera based on players
    private void SetCameraTargets()
    {
        // Get targets of players
        Transform[] targets = new Transform[m_amountofplayers];


        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_players.m_playerlist[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    //Spawns next wave when user presses next wave button
    public void btn_nextwave()
    {
        wavephase = true;
    }

    // Returns Object Manager
    public ObjectplacementManager getObjectManager()
    {
        return m_objects;
    }
}