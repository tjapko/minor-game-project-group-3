using UnityEngine;
using System;

public class TravellingSalesmanManager
{
	//public variables
	[HideInInspector]public Transform m_SpawnPoint;                      // Spawn position of base
	public GameObject m_travellingSalesmanPrefab;     // Reference to the travelling Salesman
	public GameObject m_Instance;                       // Reference to instance of base
	public GameObject m_gridprefab;                       // Reference to instance of base
	private Grid grid;

	//	private variables
	private bool metPlayer = false;
	private float travellingSalesmanDistancePercentage = 0.4f; // (0.5 - travellingSalesmanDistancePercentage) * x-dimension of the base 
	private int wavePerTravellingSalesman = 1;

	//Constructor
	public TravellingSalesmanManager (GameObject m_travellingSalesman)
	{
		this.m_travellingSalesmanPrefab = m_travellingSalesman;
	}

	void Start () {
		GridManager m_gridmanager = new GridManager(m_gridprefab);
		grid = GameObject.FindWithTag("grid").GetComponent<Grid>();
		GameObject.Destroy (m_gridmanager.m_instance, 2f);
	} 

	private Vector3 RandomSpawnPosition() 
	{
		Vector3 randomPosition;

		float buffer = 1.0f;  	// buffer for extra space between enemies and wall maybe not needed for later (walkable will fix this)
		bool walkable = true;	 
		float distance;		 	// distance between base and enemies spawnpoint 

		// base's spawning position
		Vector3 Base = GameObject.FindGameObjectWithTag ("Base").GetComponent<Transform> ().transform.position;

		// floats for holding dimensions of the map (walls)
		float x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
		float x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
		float z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
		float z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;

		// enemies needs to be spawned at least (travellingSalesmanDistancePercentage*100)% of the x-dimenion of the base
		float crit_distance  = travellingSalesmanDistancePercentage * (x_maxrange - x_minrange); 

		do {
			randomPosition = new Vector3 (UnityEngine.Random.Range (x_minrange, x_maxrange), 0f, UnityEngine.Random.Range (z_minrange, z_maxrange));
			walkable = !(Physics.CheckSphere(randomPosition, (grid.nodeRadius * 1.4f), grid.unwalkableMask));
			distance = Vector3.Distance(Base, randomPosition);
		} while (distance <= crit_distance || !walkable); // distance needs to be smaller than critical distance and the spawnpoint needs to be walkable
		return randomPosition;
	}

	//Spawn base
	public void spawnTravellingSalesman()
	{
		GameObject newTravellingSalesman = GameObject.Instantiate(m_travellingSalesmanPrefab,  RandomSpawnPosition() , m_SpawnPoint.rotation) as GameObject;
		m_Instance = newTravellingSalesman;
//		m_SpawnPoint = m_Instance.transform;
	}

	//Destroy base
	public void destroyTravellingSalesman()
	{
		GameObject.Destroy(m_Instance);
		m_Instance = null;
	}
		
	//Reset function
	public void Reset()
	{
		//Reset base position and direction
//		m_Instance.transform.position = m_SpawnPoint.position;
//		m_Instance.transform.rotation = m_SpawnPoint.rotation;

		//Reset active value
		m_Instance.SetActive(false);
		m_Instance.SetActive(true);
	}

	// getter for wavePerTravellingSalesman
	public int getWavePerTravellingSalesman() {
		return wavePerTravellingSalesman;
	}

	// getter for metPlayer
	public bool getMetPlayer() {
		return metPlayer;
	}


	public void updateMetPlayer() {
//		if (Collision.tag("player")) {
//			metPlayer = true;
//		}
	}

}
