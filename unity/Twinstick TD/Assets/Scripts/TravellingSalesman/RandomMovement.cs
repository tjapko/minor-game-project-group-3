﻿using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

	// public variables
	public int m_changeDirTimeInterval = 2; // interval in which the direction (of walking) is keeped fixed
	public int m_lifetime = 30;
	// private variables
	private float m_timer; // variable to check if the travelling Salesman has to change it's direction 
	private NavMeshAgent m_nav; // used to move the travelling Salesman
	private Vector3 m_target;   // the new target of the travelling Salesman
	private static bool m_work = false; // bool m_work checks if the Salesman has sold something 
	private float m_timer2;

	void Start () {
		m_timer = 0.0f; // restart m_timer
		m_nav = gameObject.GetComponent<NavMeshAgent> (); // search for the NavMeshAgent component of the travellingSalesman
	}

	void FixedUpdate () {
		m_timer += Time.deltaTime;  // the m_timer is updated 
		m_timer2 += Time.deltaTime;  // the m_timer is updated 
		if (m_timer >= m_changeDirTimeInterval) { // check interval 
			newTarget (); // set new target of the travellingSalesman
			m_timer = 0.0f;	// reset the m_timer
		}
		if (m_timer2 > m_lifetime || !GameManager.getWavephase()) {
			setWorkToFalse (); // Salesman removed (is not selling anymore)
			destroyTravellingSalesman ();
		}

	}

	private void newTarget() {

		float buffer = 10.0f;

		//position of the travellingSalesman:
		float x = gameObject.transform.position.x; 
		float z = gameObject.transform.position.z;

		// floats for holding dimensions of the map (walls)
		float x_minrange = GameObject.FindGameObjectWithTag ("Wall4").GetComponent<Transform> ().transform.position.x + buffer;
		float x_maxrange = GameObject.FindGameObjectWithTag ("Wall2").GetComponent<Transform> ().transform.position.x - buffer;
		float z_minrange = GameObject.FindGameObjectWithTag ("Wall1").GetComponent<Transform> ().transform.position.z + buffer;
		float z_maxrange = GameObject.FindGameObjectWithTag ("Wall3").GetComponent<Transform> ().transform.position.z - buffer;
	
		//position of the new target:
		x += Random.Range (x_minrange, x_maxrange);
		z += Random.Range (z_minrange, z_maxrange);
		//set new target
		m_target = new Vector3(x, 0.0f, z);
		// move the travellingSalesman to the new target
		m_nav.SetDestination (m_target);
	}
		
	// checks if a player has met the Salesman
	void OnTriggerEnter(Collider other) { 
		if (other.gameObject.tag == "Player") {
			setWorkToFalse (); // Salesman removed (is not selling anymore)
			destroyTravellingSalesman (); // destroy the TravellingSalesman prefab(clone) from the hierachy
		}
	}

	//Destroy the TravellingSalesman
	public void destroyTravellingSalesman()
	{
		GameObject.Destroy (this.gameObject);
	}

	// getter for m_work
	public static bool getWork() {
		return m_work;
	}

	// setter for m_work
	public static void setWorkToFalse() {
		m_work = false;
	}

	// setter for m_work
	public static void setWorkToTrue() {
		m_work = true;
	}
}
