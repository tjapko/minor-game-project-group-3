using UnityEngine;
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
    public float m_waveDelay = 15f;              // The delay between ending and starting of wave
    public float m_EndDelay = 3f;               // The delay between losing and restarting
    public AudioSource backgroundSource;
    public AudioClip[] backgroundSounds;
    public AudioSource gongSource;
    public AudioClip gongSound;

    //References
    public GameObject m_uiprefab;               // Reference to UI prefab
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
    [HideInInspector]public Transform m_Enemyspawnpoint;         // Spawnpoint of enemy
	public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.

    //Private variables
    private MapUIScript m_uiscript;             // The UI script
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


    //Start
    private void Start()
    {
       
        //Setting up variables
        m_EndWait = new WaitForSeconds(m_EndDelay);

        //Initialize managers
		m_travellingSalesmanManager = new TravellingSalesmanManager(m_travellingSalesman);
        m_players = new UserManager(m_Playerprefab, m_Playerspawnpoint, m_amountofplayers);
        

        m_base = new BaseManager(m_baseprefab, m_Basespawnpoint);
		m_gridManager = new GridManager(m_gridPrefab);

        //Initialize UI script
        m_uiscript = new MapUIScript(gameObject.GetComponent<GameManager>(), m_uiprefab, m_players);

        

           
        
        
        // Start the game
        StartCoroutine(GameLoop());

        m_wave = new WaveManager(m_Enemyprefab1, m_Enemyprefab2, m_Enemyprefab3, m_Enemyprefab4, m_Enemyspawnpoint, m_Basespawnpoint, m_players.m_playerlist[0].m_Instance.transform, m_gridPrefab);

        backgroundSource.clip = backgroundSounds[Random.Range(0, backgroundSounds.Length)];
        backgroundSource.Play();
    }

    //Check per frame
    private void Update()
    {
        //Escape key: pause menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Check if players are constructing
            //Escape button is then used to quit building
            if (!m_players.checkConstruction())
            {
                //Check if player(s) are using the shop
                GameObject shopUI = GameObject.FindWithTag("ShopUI");
                if (shopUI == null || !shopUI.activeSelf)
                {
                    gamepause = !gamepause;
                }
            }
        }
        pauseGame(gamepause);

        //Set the Object placement phase
        m_players.setConstructionphase(!wavephase);

        //Show or hide UI menu depending on wavephase and pause
        m_uiscript.UIchange(gameover, wavephase, gamepause);

        //Update score
        m_uiscript.UpdateUI();

		// if base is dead, all existing enemies move to player
		if (!m_wave.baseDead && m_base.m_Instance.activeSelf == false) {
			m_wave.baseDead = true;
			m_wave.enemiesToPlayer ();
		}
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(Startgame());
        // play sound
        


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
		//spawn base
        m_base.spawnBase();

        //Set camera
        SetCameraTargets();

        // Reset all players and enable control
        m_players.resetAllPlayers();
        m_players.enablePlayersControl();

        //m_wave.DestroyEnemies();

        //Set variables
        m_waveNumber = 0;
        wavephase = false;
        gamepause = false;
        gameover =  false;

        //Set camera
        m_CameraControl.SetStartPositionAndSize();
        
        // Wait m_StartWait of seconds before starting rounds
        yield return null;
    }

    //Play round
    private IEnumerator wavePhase()
    {
        // Clear the text from the screen.
        // m_MessageText.text = string.Empty;
        


        // Wait until base has no health or players are dead
        while (!playersDead())

        {
            wavephase = true;
            

            //Enemies are dead
            if (m_wave.EnemiesDead())
            {
				m_players.rewardPlayer();
                // Go into construction phase
                yield return StartCoroutine(constructionPhase());
				m_waveNumber++;
				StartCoroutine(m_uiscript.showWaveStatsUI());
                //Spawn next wave and remove dead enemies
                //While loop is needed, because EnemiesDead() is not fast enough to detect that a new wave has spawned
                m_wave.DestroyEnemies();
                gongSource.clip = gongSound;
                gongSource.Play();

                yield return StartCoroutine(m_wave.NextWave());

              
                
				TravellingSalesman (); // spawning of the TravellingSalesman

            }	
            // Return next frame without delay
            yield return null;
        }

		//Destroy hitcanvas when game over
		GameObject[] canvasList = GameObject.FindGameObjectsWithTag ("HitCanvas");
		foreach (GameObject element in canvasList) {
			Destroy (element);
		}

        //Player has lost
        gameover = true;
        gamepause = true;
        Debug.Log("GAME OVER");
        
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
        for(int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(m_waveDelay/100);
            
            if(wavephase)
            {
                break;
            }
        }
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
        Transform[] targets = new Transform[m_amountofplayers];


        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_players.m_playerlist[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    // Input from restart button
    public void btn_restartgame()
    {
        StartCoroutine(GameLoop());
    }

    // Spawns next wave when user presses next wave button
    public void btn_nextwave()
    {
        wavephase = true;
    }

    // Input from pause button
    public void setpause(bool status)
    {
        gamepause = status;
    }

	// spawning of the travellingSalesman
	private void TravellingSalesman() {
		// check if the Salesman needs to be spawned and not already spawned
		if (m_waveNumber % m_travellingSalesmanManager.getWavePerTravellingSalesman()==0 &&
			!m_travellingSalesmanManager.getWork()) {
			m_travellingSalesmanManager.spawnTravellingSalesman (m_gridManager.m_grid);
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

}

