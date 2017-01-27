using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//using System;


public class WaveManager
{
    //Public variables
	[HideInInspector]public Transform m_enemyspawnpoints;   //Spawnpoints of enemies
    public GameManager gameManager;

	public GameObject m_base;         //Target(s) of enemies
	public Transform m_playerpoint;
    public Transform m_target;             //Target(s) of enemies
    public GameObject m_Enemyprefab1;	//Reference to prefab of enemy1
	public GameObject m_Enemyprefab2;   //Reference to prefab of enemy2
	public GameObject m_Enemyprefab3;   //Reference to prefab of enemy3
	public GameObject m_Enemyprefab4;   //Reference to prefab of enemy4 (Boss)
	public GameObject m_gridprefab;
	public double baseDistancePercentage = 0.30; // minimal distance to travel for each enemy
    public double baseDistancePercentageBoss = 0.5; // distance to be travlled by the Boss to the base

    Grid grid;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public double baseDistanceProportion = 0.2; // minimal distance to travel for each enemy
	//public bool baseDead = false;				// if baseDead, all enemies walk towards player

    //Private variables
    private List<EnemyManager> m_enemywave; //Population of enemies
    private int enemy_number;               //Total amount of enemies spawned
	private int m_wavenumber;				//Total amount of waves
	private int m_wave;						// wave number for Enemies left counter 

    // Use this for initialization
	private GridManager m_gridmanager;
    private double m_proportionEnemy1 = 0.4; // proportion of choosing enemy1 each spawning 
    private double m_proportionEnemy2 = 0.4; // proportion of choosing enemy2 each spawning 
    private double m_proportionEnemy3; 		 // proportion of choosing enemy3 each spawning 
    private int m_numberOfWavesPerBoss = 5;  // each m_numberOfWavesPerBoss waves a Boss is spawned

	// spawnDelay of the enemies 
	private float m_spawnDelayBetweenEnemies = 0.25f; // time delay between the enemies in a wave
	// increasing amount of enemies 
	private int m_startEnemies = 5; // number of starting enemies 
	private int m_maxEnemies = 75;
	private float m_angle1 = 2.0f;     // this angle is applied from the start to wave: m_m_waveTippingPoint1
	private float m_angle2 = 3.0f;	   // this angle is applied from wave: m_m_waveTippingPoint1 to wave: m_m_waveTippingPoint2
	private float m_angle3 = 2.0f;	   // this angle is applied from wave: m_m_waveTippingPoint2 till the end of the game
	private int m_waveTippingPoint1 = 10; // wave at which m_angle2 is used for the increasing amount of enemies 
	private int m_waveTippingPoint2 = 15; // wave at which m_angle2 is used for the increasing amount of enemies 

    //GA
    public PopulationManagerGA GAManager;


	public WaveManager(GameObject Enemyprefab1, GameObject Enemyprefab2, GameObject Enemyprefab3, GameObject Enemyprefab4, Transform enemyspawnpoints, GameObject _base, Transform playerpoint, GameObject gridprefab, GridManager gridmanager)

    {
     
        this.m_Enemyprefab1 = Enemyprefab1;
        this.m_Enemyprefab2 = Enemyprefab2;
        this.m_Enemyprefab4 = Enemyprefab4;
		this.m_Enemyprefab3 = Enemyprefab3;
        this.m_enemyspawnpoints = enemyspawnpoints;
        this.m_base = _base;
        this.m_playerpoint = playerpoint;
        this.m_gridprefab = gridprefab;
		this.m_gridmanager = gridmanager;

        this.m_wavenumber = 0;
        enemy_number = 0;
		m_wave = -1;
        m_enemywave = new List<EnemyManager>();

        //		this.m_startSpawnDelayTime = this.numberEnemiesPerWave*0.5f;
        //		this.m_endSpawnDelayTime = m_scaleEnemies * 0.5f;
        //		this.time = this.m_startSpawnDelayTime;

        this.GAManager = new PopulationManagerGA(m_startEnemies);
    }


    // Send next wave and create new grid
    public IEnumerator NextWave()
	{
       
		m_wave++;
		if (m_wavenumber > 0) {
			GameObject.Destroy (m_gridmanager.m_instance, 0f);
			m_gridmanager = new GridManager (m_gridprefab);
		}
		grid = GameObject.FindWithTag ("grid").GetComponent<Grid> ();
		int enemies = EnemiesAmountPerWave ();
		GAManager.nextGenartion(enemies, m_wavenumber);   // give the current list of enemies to GA and that updates to incubation list
		m_wavenumber++;
		proportionEnemies (); // update the proportions of the enemies per wave
		yield return SpawnAllEnemies (enemies);
    }

	// Function for amount of enemies per wave
	public int EnemiesAmountPerWave() {
//		int amount =  Mathf.RoundToInt(m_wavenumber * 1.5f * m_scaleEnemies / (m_wavenumber + 10)); // m_scaleEnemies after 20 waves (*1.5 factor)
		float amount;
		float b2, b3; // off sets for graphs (lines) 2 and 3
		b2 = (m_startEnemies + m_angle1 * m_waveTippingPoint1) - (m_waveTippingPoint1 * m_angle2);
		b3 = (b2 + m_angle2 * m_waveTippingPoint2) - (m_waveTippingPoint2 * m_angle3);

		if (m_wavenumber == 0) {
			amount = m_startEnemies;
		}
		else if (m_wavenumber < m_waveTippingPoint1) {
			amount = m_startEnemies + m_angle1 * (m_wavenumber);
		} else if (m_wavenumber >= m_waveTippingPoint1 && m_wavenumber < m_waveTippingPoint2) {
			amount = m_angle2 * m_wavenumber + b2;
		} else {
			amount = m_angle3 * m_wavenumber + b3;
		}
		if (amount > m_maxEnemies) {
			return m_maxEnemies;
		}
		return (int)amount;
	}

	// Function for amount of enemies next wave
	public int EnemiesAmountPerWave2() {
		//		int amount =  Mathf.RoundToInt(m_wave * 1.5f * m_scaleEnemies / (m_wave + 10)); // m_scaleEnemies after 20 waves (*1.5 factor)
		float amount;
		float b2, b3; // off sets for graphs (lines) 2 and 3
		b2 = (m_startEnemies + m_angle1 * m_waveTippingPoint1) - (m_waveTippingPoint1 * m_angle2);
		b3 = (b2 + m_angle2 * m_waveTippingPoint2) - (m_waveTippingPoint2 * m_angle3);

		if (m_wave == 0) {
			amount = m_startEnemies;
		}
		else if (m_wave < m_waveTippingPoint1) {
			amount = m_startEnemies + m_angle1 * (m_wave);
		} else if (m_wave >= m_waveTippingPoint1 && m_wave < m_waveTippingPoint2) {
			amount = m_angle2 * m_wave + b2;
		} else {
			amount = m_angle3 * m_wave + b3;
		}
		if (amount > m_maxEnemies) {
			return m_maxEnemies;
		}

		return (int)amount;
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

           // get enemy varriables from the GA manager in GAManager.getInheratedType1()
            Enemie1 instance = new Enemie1(newinstance, m_enemyspawnpoints, m_base, m_playerpoint, enemy_number,GAManager.getInheratedType1());
            m_enemywave.Add (instance);
		} else if (Enemyprefab.Equals (m_Enemyprefab2)) {
            Enemie2 instance = new Enemie2(newinstance, m_enemyspawnpoints, m_base, m_playerpoint, enemy_number,GAManager.getInheratedType2());
            m_enemywave.Add (instance);
		} else if (Enemyprefab.Equals (m_Enemyprefab3)) {
            Enemie3 instance = new Enemie3(newinstance, m_enemyspawnpoints, m_base, m_playerpoint, enemy_number,GAManager.getInheratedType3());
            m_enemywave.Add (instance);
		} else if (boss) {
            Enemie4 instance = new Enemie4(newinstance, m_enemyspawnpoints, m_base, m_playerpoint, enemy_number,GAManager.getInheratedType4());
            instance.health.setCurrentHealth(20);

            m_enemywave.Add (instance);
		}

		//if base is dead and enemy is not already moving to player -> go to player
		//if (baseDead && !m_enemywave[m_enemywave.Count-1].m_MovementPlayer.playerFirst) {
		//	m_enemywave[m_enemywave.Count-1].m_MovementPlayer.goToPlayer ();
		//}

        enemy_number++;
	}

	// Spawning all enemies
	private IEnumerator SpawnAllEnemies(int m_number_enemies) {
        //Spawning algorithm (example)
        int enemies_spawned = 0;
//		time = spawnDelayTime ();
//		Debug.Log ("wave:" + m_wavenumber + ", amount of enemies: " + m_number_enemies);
	    while (true)
        {
            RouletteWheelSpawnEnemy();
            enemies_spawned++;
			yield return new WaitForSeconds(m_spawnDelayBetweenEnemies);
	
            if(enemies_spawned >= m_number_enemies - 1)
            {
                break;
            }
        }

		if (m_number_enemies != 1) {
	        //Spawn boss or last enemy
			if (m_wavenumber % m_numberOfWavesPerBoss == 0 && m_wavenumber != 0) { // wavenumber needs to start at zero
				// spawning the Boss:
				InstatiateEnemy(m_Enemyprefab4, true); // for spawning the boss 
			}
			else {
					RouletteWheelSpawnEnemy (); 
				}

	        yield return null;
			}
		}
		
	//Check if all enemies are dead;
	public bool EnemiesDead()
	{
		foreach (EnemyManager enemy in m_enemywave)
		{
			if (enemy.m_Instance != null && enemy.m_Instance.activeSelf) {
				return false;
			}
		}

        UpdateEnemyPreformance();


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

		//baseDead = false;
    }

    public void UpdateEnemyPreformance()
    {
        foreach (EnemyManager enemy in m_enemywave)
        {
            enemy.updatePreformance();
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

//	// returning spawnTime (varies per wave)
//	private float spawnDelayTime() {  
////		float m_timeExtra; // added amount of time to the total spawnTime of the wave 
//		if (m_wavenumber != 1) { 
////			m_timeExtra = 1.0f / scaleTime;
////			time += m_timeExtra;
//			time = EnemiesAmountPerWave()*0.5f;
//		}
//		if (time > m_endSpawnDelayTime) {
//			time = m_endSpawnDelayTime;
//		}
//		return time;
//	}

    private int sethealt()
    {
        //Example algorithm
        return (int)  (Mathf.Pow(1.5f    , m_wavenumber)); // updating hp enemy per wave
    }

    //Returns amount of enemies remaining 
    public int enemiesRemaining()
    {
        return EnemiesAmountPerWave2() - countDeadEnemies();
    }
    
    //Counts the amount of dead enemies
    private int countDeadEnemies()
    {
        if(m_enemywave == null)
        {
            return 0;
        }

        int ans = 0;
        foreach (EnemyManager enemy in m_enemywave)
        {
            if (!enemy.m_Instance.activeSelf)
            {
                ans++;
            }
        }
        return ans;
    } 

}
