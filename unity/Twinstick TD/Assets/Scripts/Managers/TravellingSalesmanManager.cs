using UnityEngine;
using System;
using System.Collections.Generic;

public class TravellingSalesmanManager
{
	//public variables
	public GameObject m_travellingSalesmanPrefab;       // Reference to the travelling Salesman
	public GameObject m_Instance;                       // Reference to instance of salesman 

	//	private variables
	private bool metPlayer = false;  // if the Salesman has met the player this value is turned true
	private float travellingSalesmanDistancePercentage = 0.4f; // (0.5 - travellingSalesmanDistancePercentage) * x-dimension of the base 
	private int wavePerTravellingSalesman = 1; // number of waves per Travelling Salesman

	//Constructor
	public TravellingSalesmanManager (GameObject m_travellingSalesman)
	{
		this.m_travellingSalesmanPrefab = m_travellingSalesman;
	}
		
	// Determining random spawnposition (with extra conditions) 
	public Vector3 RandomSpawnPosition(Grid grid) 
	{
		Vector3 randomPosition;

		float buffer = 1.0f;  	// buffer for extra space between Salesman and wall maybe not needed for later (walkable will fix this)
		bool walkable = true;	 
		float distance;		 	// distance between base and Salesman spawnpoint 

		// base's spawning position
		Vector3 Base = GameObject.FindGameObjectWithTag ("Base").GetComponent<Transform> ().transform.position;

		// floats for holding dimensions of the map (walls)
		float x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
		float x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
		float z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
		float z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;

		// Salesman needs to be spawned at least (travellingSalesmanDistancePercentage*100)% of the x-dimenion of the base
		float crit_distance  = travellingSalesmanDistancePercentage * (x_maxrange - x_minrange); 

		do {
			randomPosition = new Vector3 (UnityEngine.Random.Range (x_minrange, x_maxrange), 0f, UnityEngine.Random.Range (z_minrange, z_maxrange));
			walkable = !(Physics.CheckSphere(randomPosition, (grid.nodeRadius * 1.4f), grid.unwalkableMask));
			distance = Vector3.Distance(Base, randomPosition);
		} while (distance <= crit_distance || !walkable); // distance needs to be smaller than critical distance and the spawnpoint needs to be walkable
		return randomPosition;
	}

	//Spawn TravellingSalesman
	public void spawnTravellingSalesman(Grid grid)
	{
		m_travellingSalesmanPrefab.transform.position = RandomSpawnPosition(grid);
		GameObject newTravellingSalesman = GameObject.Instantiate(m_travellingSalesmanPrefab.gameObject) as GameObject;
		m_Instance = newTravellingSalesman;
		RandomMovement.setWorkToTrue(); // Added "working/selling" Salesman 
	}
				
	// getter for wavePerTravellingSalesman
	public int getWavePerTravellingSalesman() {
		return wavePerTravellingSalesman;
	}

	// getter for metPlayer
	public bool getMetPlayer() {
		return metPlayer;
	}

	// getter for m_work from RandomMovement 
	public bool getWork() {
		return RandomMovement.getWork ();
	}

}