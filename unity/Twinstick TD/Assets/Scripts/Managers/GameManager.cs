﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

//UNDER CONSTRUCTION
/// <summary>
/// Class GameManager
/// </summary>
public class GameManager : MonoBehaviour
{
    //Public variables
    public int m_amountofplayers;               // Total amount of players that are participating
//  public float m_beginDelay = 3;
    public static float m_waveDelay = 120f;              // The delay between ending and starting of wave
    public static float m_EndDelay = 3f;               // The delay between losing and restarting
    public AudioSource backgroundSource;
    public AudioClip[] backgroundSounds;
    public AudioSource gongSource;
    public AudioClip gongSound;

    //References
    public GameObject m_baseprefab;             // Reference to the base
    public GameObject m_Playerprefab;           // Reference to the prefab the players will control.
	public GameObject m_Enemyprefab1;       //Reference to prefab of enemy1
	public GameObject m_Enemyprefab2;       //Reference to prefab of enemy2
	public GameObject m_Enemyprefab3;       //Reference to prefab of enemy3
	public GameObject m_Enemyprefab4;       //Reference to prefab of enemy14 (Boss)

    public GameObject m_gridPrefab;             // Reference to the prefab of the grid and path
	public GameObject m_travellingSalesman;     // Reference to the travelling Salesman
    public Transform m_Basespawnpoint;          // Spawnpoint of base
    public Transform m_Playerspawnpoint;        // Spawnpoint of player
    public Transform m_Enemyspawnpoint;         // Spawnpoint of enemy
	public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public GameObject m_weaponshop;             // Reference to the weapon shop                 

    //Private variables
    private UIManagerV2 m_uiscript;             // The UI script
    private BaseManager m_base;                 // The base manager of the base
    private UserManager m_players;              // A collection of managers for enabling and disabling different aspects of the players.
    private WaveManager m_wave;                 // A collection of managers for enabling and disabling different aspects of the enemies.
    private GridManager m_gridManager;          // Script gridmanager
	private TravellingSalesmanManager m_travellingSalesmanManager;     // The manager of the travelling Salesman 
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private int m_waveNumber;                   // Which wave the game is currently on.
    private bool gameover;                      // Boolean if game is over
    private bool gamepause;                     // Boolean if game is paused
    private static bool wavephase;                     // Boolean if game is in wavephase or construction phase 
	public static int m_waveNumberusedForHighscore;                   

    //Start
    private void Start()
    {
        //Setting up variables
//        m_StartWait = new WaitForSeconds(m_beginDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        Time.timeScale = 1.0f;

        //Initialize managers
		m_travellingSalesmanManager = new TravellingSalesmanManager(m_travellingSalesman);
        m_players = new UserManager(m_Playerprefab, m_Playerspawnpoint, m_amountofplayers);
        
        m_base = new BaseManager(m_baseprefab, m_Basespawnpoint);
		m_gridManager = new GridManager(m_gridPrefab);

        //Initialize UI script
        m_uiscript = GameObject.FindWithTag("UIManager").GetComponent<UIManagerV2>();

        // Start the game
        StartCoroutine(GameLoop());

		m_wave = new WaveManager(m_Enemyprefab1, m_Enemyprefab2, m_Enemyprefab3, m_Enemyprefab4, m_Enemyspawnpoint, m_base.m_Instance, m_players.m_playerlist[0].m_Instance.transform, m_gridPrefab, m_gridManager);

        backgroundSource.clip = backgroundSounds[Random.Range(0, backgroundSounds.Length)];
        backgroundSource.Play();
    }

    //Check per frame
    private void Update()
    {
        //Escape key: pause menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Check for game over
            if (!gameover)
            {

                //Check if players are constructing
                if(m_players.checkConstruction()){

                }
                else
                {
                    gamepause = !gamepause;
                    pauseGame(gamepause);
                    m_uiscript.UIchange(gameover, wavephase, gamepause);
                }
            }
            
        }

        //pauseGame(gamepause);
      
        //Set the Object placement phase
        m_players.setConstructionphase(!wavephase);
      
        //Show or hide UI menu depending on wavephase and pause
        m_uiscript.UIchange(gameover, wavephase, gamepause);
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        //Initialize other game objects
        m_base.spawnBase();
        m_uiscript.StartInitialization();   //Initialize UI script

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
		// when pressed restart (m_wave not build yet), destroy enemies
		if (m_wave != null) {
			m_wave.DestroyEnemies ();
		}


        //Spawning base and users
        m_players.resetAllPlayers();
        //m_players.destroyPlayers();
		if (m_players.m_playerlist.Count == 0) {
			m_players.spawnPlayers ();
		}

        //Set camera
        SetCameraTargets();

        // Reset all players and enable control
        m_players.resetAllPlayers();
        m_players.enablePlayersControl();

        //m_wave.DestroyEnemies();

        //Set variables
        m_waveNumber = 0;
		m_waveNumberusedForHighscore = 0;
        wavephase = true;
        gamepause = false;
        gameover =  false;

        //Set canvas
        m_players.setConstructionphase(!wavephase);
        m_uiscript.UIchange(gameover, wavephase, gamepause);

        //Set camera
        m_CameraControl.SetStartPositionAndSize();
        
        // Wait m_StartWait of seconds before starting rounds
        yield return null;
    }

    //Play round
    private IEnumerator wavePhase()
    {
        // Wait until base has no health or players are dead
        while (!playersDead())
        {

            wavephase = true;
            m_players.setConstructionphase(!wavephase);
            m_uiscript.UIchange(gameover, wavephase, gamepause);


            //Enemies are dead
            if (m_wave.EnemiesDead())
            {
                //Reward player
                waveReward();
                m_uiscript.showWaveReward();

                // Go into construction phase
                if (m_waveNumber == 0)
                {
                    yield return null;
                }
                else
                {
                    yield return StartCoroutine(constructionPhase());
                }

                // switch Main camera to construction camera 
                CameraControl.switchConstructionCamToMainCam();

                //Do wave number things
                m_waveNumber++;
				m_waveNumberusedForHighscore++;
                m_uiscript.setWaveNumber();

                //Spawn next wave and remove dead enemies
                //While loop is needed, because EnemiesDead() is not fast enough to detect that a new wave has spawned
                m_wave.DestroyEnemies();
                gongSource.clip = gongSound;
                gongSource.Play();

                yield return StartCoroutine(m_wave.NextWave());
                
				//TravellingSalesman (); // spawning of the TravellingSalesman

            }	
            // Return next frame without delay
            yield return null;
        }

		//Destroy hitcanvas when game over
		GameObject[] canvasList = GameObject.FindGameObjectsWithTag ("HitCanvas");
		foreach (GameObject element in canvasList) {
			Destroy (element);
		}

        Analytics.CustomEvent("GameOVer", new Dictionary<string, object> {
            {"wave number", m_waveNumber },
            {"average damage per attack poptype 1", m_wave.GAManager.poptype1.debugAverageStats()[0] },
            {"average attack speed poptype 1", m_wave.GAManager.poptype1.debugAverageStats()[1] },
            {"average health poptype 1", m_wave.GAManager.poptype1.debugAverageStats()[2] },
            {"average movementspeed poptype 1", m_wave.GAManager.poptype1.debugAverageStats()[3] },
            {"average damage per attack poptype 2", m_wave.GAManager.poptype2.debugAverageStats()[0] },
            {"average attack speed poptype 2", m_wave.GAManager.poptype2.debugAverageStats()[1] },
            {"average health poptype 2", m_wave.GAManager.poptype2.debugAverageStats()[2] },
            {"average movementspeed poptype 2", m_wave.GAManager.poptype2.debugAverageStats()[3] },
            {"average damage per attack poptype 3", m_wave.GAManager.poptype3.debugAverageStats()[0] },
            {"average attack speed poptype 3", m_wave.GAManager.poptype3.debugAverageStats()[1] },
            {"average health poptype 3", m_wave.GAManager.poptype3.debugAverageStats()[2] },
            {"average movementspeed poptype 3", m_wave.GAManager.poptype3.debugAverageStats()[3] }
                   });

        //Player has lost
        gameover = true;
        gamepause = true;
        m_uiscript.UIchange(gameover, wavephase, gamepause);
        m_players.setConstructionphase(!wavephase);
        pauseGame(gamepause);

//        Debug.Log("GAME OVER");
        
    }

	// called in wavePhase untill playersDead is true
	private bool playersDead(){
		foreach (PlayerManager player in m_players.m_playerlist) {
			if (!player.m_playerhealth.m_Dead) {
				return false;
			}
		}
		return true;
	}

    // Construction phase
    private IEnumerator constructionPhase()
    {
		// switch Main camera to construction camera 
		CameraControl.switchMainCamToConstructionCam ();

        //Set wavephase to false and set timer
        wavephase = false;
        m_players.setConstructionphase(!wavephase);
        m_uiscript.UIchange(gameover, wavephase, gamepause);

        StartCoroutine(constructionphaseTimer());
        //While the game is in construction phase perform actions
        //Can get out of while loop by getting signal from next wave button
        //Can get out of while loop when timer reaches time 
        // Timer must be shorter than time of ending next wave, incase user presses next wave button !!!! (needs fix)
        while(!wavephase)
        {
//			m_players.disablePlayersControl2();
            yield return null;
        }

        //Starting wave phase
    }
    
    // Constuction phase countdown
    private IEnumerator constructionphaseTimer()
    {
        for(int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(m_waveDelay/100);
            
            if(wavephase)
            {
                break;
            }
        }

        wavephase = true;
        m_players.setConstructionphase(!wavephase);
        m_uiscript.UIchange(gameover, wavephase, gamepause);

//		m_players.enablePlayersControl2();

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
        m_uiscript.setScore();
        // Stop players and waves from moving.
        m_players.disablePlayersControl();
        m_wave.DisableEnemyWaveControl();

        yield return m_EndWait;
    }

    //Sets position of camera based on players
    private void SetCameraTargets()
    {
        // Get targets of players
        Transform[] targets = new Transform[m_amountofplayers+1];


        for (int i = 0; i < targets.Length-1; i++)
        {
            targets[i] = m_players.m_playerlist[i].m_Instance.transform;
        }
        targets[targets.Length - 1] = m_base.m_Instance.transform;
        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    //Reward player
    private void waveReward()
    {
        if (m_base.m_Instance.activeSelf)
        {
            m_players.rewardPlayer();
        }
    }

    // Input from restart button
    public void btn_restartgame()
    {
        //Reset variables
        wavephase = false;
        gameover = false;
        gamepause = false;
        pauseGame(gamepause);   //Reset game pause

        //Reset canvas
        m_uiscript.UIchange(gameover, wavephase, gamepause);
        m_weaponshop.GetComponent<ShopScript>().resetShop();

        //Start new game
		CameraControl.restartCam();
        StartCoroutine(GameLoop());
    }

    // Spawns next wave when user presses next wave button
    public void btn_nextwave()
    {
        wavephase = true;
        m_players.setConstructionphase(!wavephase);
        m_uiscript.UIchange(gameover, wavephase, gamepause);
    }

    // Input from pause button
    public void setpause(bool status)
    {
        gamepause = status;
        pauseGame(gamepause);
        m_uiscript.UIchange(gameover, wavephase, gamepause);
    }

	// spawning of the travellingSalesman
	private void TravellingSalesman() {
		// check if the Salesman needs to be spawned and not already spawned
		if (m_waveNumber % m_travellingSalesmanManager.getWavePerTravellingSalesman()==0 &&
			!m_travellingSalesmanManager.getWork()) {
			//m_travellingSalesmanManager.spawnTravellingSalesman (m_gridManager.m_grid); // commented to disable the salesman
		}
	}

	// getter for wavephase
	public static bool getWavephase() {
		return wavephase;
	}

    //Get Usermanager
    public UserManager getUserManager()
    {
        return m_players;
    }

    //Getter for the wave number
    public int getWaveNumber()
    {
        return m_waveNumber;
    }

    //Getter Wave Manager
    public WaveManager getWaveManager()
    {
        return m_wave;
    }

    //Getter Base Manager
    public BaseManager getBaseManager()
    {
        return m_base;
    }

}

