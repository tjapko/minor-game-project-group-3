using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//using System;


public class WaveManager
{
    //Public variables
	[HideInInspector]public Transform m_enemyspawnpoints;   //Spawnpoints of enemies
    public GameManager gameManager;

    public Transform m_basetarget;             //Target(s) of enemies
	public Transform m_playerpoint;
    public Transform m_target;             //Target(s) of enemies
    public double baseDistancePercentage = 0.1;
    public GameObject m_Enemyprefab1;	//Reference to prefab of enemy1
	public GameObject m_Enemyprefab2;   //Reference to prefab of enemy2
	public GameObject m_Enemyprefab3;   //Reference to prefab of enemy3
	public GameObject m_Enemyprefab4;   //Reference to prefab of enemy14 (Boss)
	public GameObject m_gridprefab;
    public double baseDistancePercentageBoss = 0.5; // distance to be travlled by the Boss to the base

    Grid grid;
    public LayerMask unwalkableMask;
    public float nodeRadius;

	public int numberEnemiesPerWave = 25; 		//Start amount of enemies per wave
    public double baseDistanceProportion = 0.2; // minimal distance to travel for each enemy
	public bool baseDead = false;				// if baseDead, all enemies walk towards player

    //Private variables
    private List<EnemyManager> m_enemywave; //Population of enemies
    private int enemy_number;               //Total amount of enemies spawned
	private int m_wavenumber;				//Total amount of waves

    // Use this for initialization
	private GridManager m_gridmanager;
    private double m_proportionEnemy1 = 0.4; // proportion of choosing enemy1 each spawning 
    private double m_proportionEnemy2 = 0.4; // proportion of choosing enemy2 each spawning 
    private double m_proportionEnemy3; // proportion of choosing enemy3 each spawning 
    private int m_numberOfWavesPerBoss = 1;  // each m_numberOfWavesPerBoss waves a Boss is spawned
	private float m_clusterProportion = 1.0f;

    public WaveManager(GameObject Enemyprefab1, GameObject Enemyprefab2, GameObject Enemyprefab3, GameObject Enemyprefab4, Transform enemyspawnpoints, Transform basetarget, Transform playerpoint, GameObject gridprefab)
    {
     
        this.m_Enemyprefab1 = Enemyprefab1;
        this.m_Enemyprefab2 = Enemyprefab2;
        this.m_Enemyprefab4 = Enemyprefab4;
		this.m_Enemyprefab3 = Enemyprefab3;
        this.m_enemyspawnpoints = enemyspawnpoints;
        this.m_basetarget = basetarget;
        this.m_playerpoint = playerpoint;
        this.m_gridprefab = gridprefab;

        this.m_wavenumber = 0;
        this.numberEnemiesPerWave = 10;
        enemy_number = 0;
        m_enemywave = new List<EnemyManager>();
    }


    // Send next wave and create new grid
    public IEnumerator NextWave()
	{
		if (m_wavenumber == 0) {
			m_gridmanager = new GridManager (m_gridprefab);
			grid = GameObject.FindWithTag ("grid").GetComponent<Grid> ();
		}
		if (m_wavenumber > 0) {
			GameObject.Destroy (m_gridmanager.m_instance, 0f);
			m_gridmanager = new GridManager (m_gridprefab);
			grid = GameObject.FindWithTag ("grid").GetComponent<Grid> ();
		}
		int enemies = numberEnemiesPerWave + EnemiesAmountPerWave ();


		proportionEnemies (); // update the proportions of the enemies per wave

        
        yield return SpawnAllEnemies(enemies);
		m_wavenumber++;
    }

	// Function for amount of enemies next wave
	public int EnemiesAmountPerWave(){
		return m_wavenumber*2;
	}
		
	// produces a random spawnpoint for the enemy
	private Vector3 RandomSpawnPosition(bool boss) 
	{
		Vector3 randomPosition;
		Vector3 randomNodePosition;

		float buffer = 1.0f;  	// buffer for extra space between enemies and wall maybe not needed for later (walkable will fix this)
		bool walkable;
		float distance;		 	// distance between base and enemies spawnpoint 
		float crit_distance;	// minimal distance to be travelled by the enemies 

		// base's spawning position
		Vector3 Base = GameObject.FindGameObjectWithTag ("Base").GetComponent<Transform> ().transform.position;

		// floats for holding dimensions of the map (walls)
		float x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
		float x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
		float z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
		float z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;

		if (!boss) {
			// enemies needs to be spawned at least (baseDistancePercentage*100)% of the x-dimenion of the base
			crit_distance = (float)baseDistancePercentage * (x_maxrange - x_minrange); 
		} else {
			// boss needs to be spawned at least (baseDistancePercentage*100)% of the x-dimenion of the base
			crit_distance = (float)baseDistancePercentageBoss * (x_maxrange - x_minrange); 
		}

		do {
			randomPosition = new Vector3 (Random.Range (x_minrange, x_maxrange), 0f, Random.Range (z_minrange, z_maxrange));
			// from random World position to random (center of) Node position:
			Node node = grid.NodeFromWorldPoint (randomPosition);
			randomNodePosition = node.worldPosition;

			walkable = !(Physics.CheckSphere(randomNodePosition, (grid.nodeRadius * 1.4f), grid.unwalkableMask));
			distance = Vector3.Distance(Base, randomNodePosition);
		} while (distance <= crit_distance || !walkable); // distance needs to be smaller than critical distance and the spawnpoint needs to be walkable
		return randomNodePosition;
	}
		
	// Roulette-wheel function 
	private void RouletteWheelSpawnEnemy() {

		double rnd = Random.value;

		if (rnd < m_proportionEnemy1) {
			// instantiate enemy type 1
			InstatiateEnemy(m_Enemyprefab1, false);
		} else if (rnd - m_proportionEnemy1 < m_proportionEnemy2) {
			// instantiate enemy type 2
			InstatiateEnemy(m_Enemyprefab2, false);
		} else {
			// instantiate enemy type 3
			InstatiateEnemy(m_Enemyprefab3, false);
		}

	}

	// instantiate enemy
	private void InstatiateEnemy (GameObject Enemyprefab, bool boss) {
		m_enemyspawnpoints.position = RandomSpawnPosition (boss);
		GameObject newinstance = GameObject.Instantiate (Enemyprefab, m_enemyspawnpoints.position, m_enemyspawnpoints.rotation) as GameObject;

		if (Enemyprefab.Equals (m_Enemyprefab1)) {
			m_enemywave.Add (new Enemie1 (newinstance, m_enemyspawnpoints, m_basetarget, m_playerpoint, enemy_number));
		} else if (Enemyprefab.Equals (m_Enemyprefab2)) {
			m_enemywave.Add (new Enemie2 (newinstance, m_enemyspawnpoints, m_basetarget, m_playerpoint, enemy_number));
		} else if (Enemyprefab.Equals (m_Enemyprefab3)) {
			m_enemywave.Add (new Enemie3 (newinstance, m_enemyspawnpoints, m_basetarget, m_playerpoint, enemy_number));
		} else if (boss) {
			m_enemywave.Add (new Enemie4 (newinstance, m_enemyspawnpoints, m_basetarget, m_playerpoint, enemy_number));
		}

		//if base is dead and enemy is not already moving to player -> go to player
		if (baseDead && !m_enemywave[m_enemywave.Count-1].m_MovementPlayer.playerFirst) {
			m_enemywave[m_enemywave.Count-1].m_MovementPlayer.goToPlayer ();
		}

        enemy_number++;
	}

	// Spawning all enemies
	private IEnumerator SpawnAllEnemies(int m_number_enemies) {
        //Spawning algorithm (example)
        int enemies_spawned = 0;
        while (true)
        {
            if(enemiesPresent() < m_number_enemies * m_clusterProportion)
            {
                RouletteWheelSpawnEnemy();
                enemies_spawned++;
            }

            if(enemies_spawned >= m_number_enemies - 1)
            {
                break;
            }

            yield return null;
        }

        //Spawn boss or last enemy
		if (m_wavenumber % m_numberOfWavesPerBoss == 0 && m_wavenumber >0) { // wavenumber needs to start at zero
			// spawning the Boss:
			InstatiateEnemy(m_Enemyprefab4, true); // for spawning the boss 
		}
		else {
				RouletteWheelSpawnEnemy (); 
			}

        yield return null;
		}
		
	//Check if all enemies are dead;
	public bool EnemiesDead()
	{
		foreach (EnemyManager enemy in m_enemywave)
		{
			if (enemy.m_Instance.activeSelf)
			{
				return false;
			}
		}
		return true;
	}
		
    //Remove dead enemies
    public void DestroyEnemies()
    {
		int n = m_enemywave.Count;
        for (int i = 0; i < n; i++)
        {
            GameObject.Destroy(m_enemywave[0].m_Instance);
            m_enemywave.RemoveAt(0);
        }

    }

    //Enable control of enemies
    public void EnableEnemyWaveControl()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            enemy.EnableControl();
        }
    }

    //Disable control of enemies
    public void DisableEnemyWaveControl()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            enemy.DisableControl();
        }
    }
		
	// proportions adjusments when m_wavenumber is increasing 
	private void proportionEnemies () {
		// proportion of enemy1 decreases each wave
		m_proportionEnemy1 = m_proportionEnemy1 - m_wavenumber / 100.0;
		if (m_proportionEnemy1 < 0.2) {
			m_proportionEnemy1 = 0.2;
		}
		// proportion of enemy2 decreases each wave
		m_proportionEnemy2 = m_proportionEnemy2 - m_wavenumber / 100.0;
		if (m_proportionEnemy2 < 0.2) {
			m_proportionEnemy2 = 0.2;
		}
		// proportion of enemy3 increases each wave (because m_proportionEnemy1 & m_proportionEnemy2 both decrease each wave)
		m_proportionEnemy3 = 1 - m_proportionEnemy1 - m_proportionEnemy2;
	}

    //Amount of enemies are present 
    private int enemiesPresent()
    {
        int answer = 0;
        foreach (EnemyManager enemy in m_enemywave)
        {
            if (enemy.m_Instance.activeSelf)
            {
                answer++;
            }
        }

        return answer;
    }

	// for all existing enemies in m_enemywave, let them walk to player
	public void enemiesToPlayer(){
		foreach (var enemy in m_enemywave) {
			if (baseDead && !enemy.m_MovementPlayer.playerFirst) {
				enemy.m_MovementPlayer.goToPlayer ();
			}
		}
	}
}
