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
    public float m_StartDelay = 3f;             // The delay between the start of round and playing of round
    public float m_waveDelay = 5f;              // The delay between ending and starting of wave
    public float m_EndDelay = 3f;               // The delay between losing and restarting
    public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public GameObject m_baseprefab;             // Reference to the base
    public GameObject m_Playerprefab;           // Reference to the prefab the players will control.
    public GameObject m_Enemyprefab;            // Reference to the prefab of the enemies.
    public BaseManager m_base;                  // The base manager of the base
    public PlayerManager[] m_players;           // A collection of managers for enabling and disabling different aspects of the player.
    public Transform Enemyspawnpoint;           // Spawnpoint of enemy

    //Private variables
    private WaveManager m_wave;                 // A collection of managers for enabling and disabling different aspects of the enemies.
    private int m_waveNumber;                   // Which wave the game is currently on.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_waveWait;          // Time between waves (not yet used)
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.

    //Start
    private void Start()
    {
        //Setting up variables
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_waveNumber = 0;
        this.m_wave = new WaveManager(m_Enemyprefab, Enemyspawnpoint, m_base.m_SpawnPoint);

        //Spawning base
        spawnbase();
        spawnAllPlayers();

        SetCameraTargets();

        // Once the players and base has been created start game
        StartCoroutine(GameLoop());

		// Initializes the statistics of the players (Currency, kills, etc)
		for (int i = 0; i < m_players.Length; i++) 
		{
			m_players [i].Start ();
		}
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

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }
		
	// Rounds of the game
    private IEnumerator RoundPlaying()
    {
        //Increase wave number


        // Clear the text from the screen.
        // m_MessageText.text = string.Empty;

        //Send next wave and increase wave number
        //While loop is needed, because EnemiesDead() is not fast enough to detect that a new wave has spawned
        while (m_wave.EnemiesDead())
        {
            m_wave.NextWave();
            m_waveNumber++;
        }


        // While there is not one tank left...
        while (!(playerDead() || m_wave.EnemiesDead()))
        {
            // ... return on the next frame.
            yield return null;
        }

        //Remove all dead enemies
        m_wave.DestroyEnemies();


        Debug.Log("Current wave" + m_waveNumber);

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

    // Reset player position(tested)
    private void resetAllPlayers()
    {
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i].Reset();
        }
    }

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

	// Update is called for every player to update their statistics
	void Update ()
	{
		for (int i = 0; i < m_players.Length; i++) {
			m_players [i].updateStatistics ();
		} 
	}

}