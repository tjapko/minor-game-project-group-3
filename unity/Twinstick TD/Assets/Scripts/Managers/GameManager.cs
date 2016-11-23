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
    public float m_StartDelay = 3f;             // The delay between the start of round and playing of round
    public float m_waveDelay = 2f;              // The delay between ending and starting of wave
    public float m_EndDelay = 3f;               // The delay between losing and restarting
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public GameObject m_baseprefab;             // Reference to the base
    public GameObject m_Playerprefab;           // Reference to the prefab the players will control.
    public GameObject m_Enemyprefab;            // Reference to the prefab of the enemies.
    public Transform m_Basespawnpoint;            // Spawnpoint of base
    public Transform m_Playerspawnpoint;          // Spawnpoint of player
    public Transform m_Enemyspawnpoint;           // Spawnpoint of enemy

    //Private variables
    private BaseManager m_base;                 // The base manager of the base
    private UserManager m_players;              // A collection of managers for enabling and disabling different aspects of the players.
    private WaveManager m_wave;                 // A collection of managers for enabling and disabling different aspects of the enemies.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_waveWait;          // Time between waves (not yet used)
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private int m_waveNumber;                   // Which wave the game is currently on.
    bool gamepause;                             // Boolean if game is paused 

    //Start
    private void Start()
    {
        //Setting up variables
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_waveWait = new WaitForSeconds(m_waveDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_waveNumber = 0;
        gamepause = false;

        //Initialize managers
        m_wave = new WaveManager(m_Enemyprefab, m_Enemyspawnpoint, m_Basespawnpoint);
        m_players = new UserManager(m_Playerprefab, m_Playerspawnpoint, m_amountofplayers);
        m_base = new BaseManager(m_baseprefab, m_Basespawnpoint);

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
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.

        yield return StartCoroutine(Startgame());

        //Play round
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

    }

    // Starting game
    private IEnumerator Startgame()
    {
        // Reset all players and enable control
        m_players.resetAllPlayers();
        m_players.enablePlayersControl();

        //m_CameraControl.SetStartPositionAndSize();

        // Wait m_StartWait of seconds before starting rounds
        yield return m_StartWait;
    }

    //Play round
    private IEnumerator RoundPlaying()
    {
        // Clear the text from the screen.
        // m_MessageText.text = string.Empty;

        //Send next wave and increase wave number
        //While loop is needed, because EnemiesDead() is not fast enough to detect that a new wave has spawned
        while (m_wave.EnemiesDead())
        {
            m_wave.NextWave();
        }
        m_waveNumber++;
        Debug.Log("Current wave" + m_waveNumber);

        // Wait until base has no health or players are dead
        while (!(m_players.playerDead() || m_base.BaseDead()))
        {
            //Enemies are dead
            if (m_wave.EnemiesDead())
            {
                // Start wave cooldown
                StartCoroutine(RoundWaveCooldown());

                //Spawn next wave and remove dead enemies
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

    //Wave cooldown
    private IEnumerator RoundWaveCooldown()
    {
        yield return m_waveWait;
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
        // Stop tanks from moving.
        m_players.disablePlayersControl();
        m_wave.DisableEnemyWaveControl();

        yield return m_EndWait;
    }

    //Sets position of camera based on players
    private void SetCameraTargets()
    {
        // Create a collection of transforms the same size as the number of tanks.
        Transform[] targets = new Transform[m_amountofplayers];

        // For each of these transforms...
        for (int i = 0; i < targets.Length; i++)
        {
            // ... set it to the appropriate tank transform.
            targets[i] = m_players.m_playerlist[i].m_Instance.transform;
        }

        // These are the targets the camera should follow.
        m_CameraControl.m_Targets = targets;
    }

    

    

    


}