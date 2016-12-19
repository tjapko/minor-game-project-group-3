﻿using UnityEngine;
using System.Collections;

/// <summary>
/// A unit that moves from his original point to the target point 
/// </summary>
public class UnitPlayer : MonoBehaviour {

	public Transform m_base;  // the baselocation 
	public Transform m_player;// the playerlocation
	public float speed = 5f; 	// moving speed
	public bool playerFirst; 	// walking to player first or not
	public bool baseHit = false; 				// has hit the base or not
	public float m_threshold = 0f; // maybe variable for GA
	private GameObject Base;

	Vector3[] path; // The walkable path
	int targetIndex;// The index of the waypointArray. The unit moves to path[targetIndex]  



	//For enemy 3, it calculates distance to base and player and chooses closest as target
	public void calcDistance(){
		float distToPlayer = Vector3.Distance (transform.position, m_player.position);
		float distToBase = Vector3.Distance (transform.position, m_base.position);
		if (GameObject.FindGameObjectWithTag("Base1") != null) {
			if (distToBase + m_threshold <= distToPlayer) {
				goToBase ();
			} else {
				goToPlayer ();
			}
		} else {
			goToPlayer ();
		}
	}

	//if base is dead when spawn, go to player
	public void Update(){
		Debug.Log (playerFirst + " " + GameObject.Find("hoi"));
		if (!playerFirst && GameObject.FindGameObjectWithTag("Base1") == null) {
			goToPlayer ();
		}
	}

	//Starts the function walkToPlayer every 1 second
	public void goToPlayer(){
		playerFirst = true;
		InvokeRepeating ("walkToPlayer", 0f, 1f);
	}

	//Calculates path to base and walks towards
	public void goToBase(){
		playerFirst = false;
		PathRequestManager.RequestPath (transform.position, m_base.position, OnPathFound);
	}

	//Calculates path to player and walks towards till distance is small enough when it walks to player first
	public void walkToPlayer(){
		PathRequestManager.RequestPath (transform.position, m_player.position, OnPathFound);
	}

	/// <summary>
	/// If there is a path the unit will move over it
	/// </summary>
	/// <param name="newPath"></param>
	/// <param name="pathSuccessful"></param>
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			if (gameObject.activeSelf == true) {
				StopCoroutine("FollowPath");
				StartCoroutine ("FollowPath");
			}
		}
	}

	/// <summary>
	/// a coroutine for walking over the path that is given 
	/// </summary>
	IEnumerator FollowPath() {
		if (path.Length == 0) {
			yield break;
		}
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.LookAt(currentWaypoint);
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			if (!playerFirst && baseHit) {
				Debug.Log ("break deze shit");
				yield break;
			}
			yield return null;
		}
	}

	/// <summary>
	/// Visualizing the path with squares on the waypoints 
	/// </summary>
	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
