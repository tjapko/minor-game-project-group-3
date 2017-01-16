﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A unit that moves from his original point to the target point 
/// </summary>
public class UnitPlayer : MonoBehaviour {

	public Transform m_base;  			// the baselocation 
	public Transform m_player;			// the playerlocation
	public float speed; 				// moving speed
	public bool playerFirst; 			// walking to player first or not
	public bool baseHit = false; 		// has hit the base or not
	public float m_threshold = -20.0f; 	// maybe variable for GA
	public float timeNewPath = 2f; 		// interval between new pathcalculation to enemy
	public float mudSpeed = 2f;
	public float normalSpeed = 6f;
	public float bossSpeed = 3f;

	Vector3[] path; 					// The walkable path
	int targetIndex;					// The index of the waypointArray. The unit moves to path[targetIndex]  

	//Calculates distance to each playerObject in the scene and chooses closest as target
	public void calcDistance(bool secondCheck){
		float smallestDist = 10000f;
		GameObject currentGoal = null;
		List<GameObject> objects = getObjects ();
		foreach (GameObject playerObject in objects){
			float dist = Vector3.Distance (playerObject.transform.position, transform.position);
			if (dist < smallestDist) {
				smallestDist = dist;
				currentGoal = playerObject;
			}
		}
		if (currentGoal != null) {
			PathRequestManager.RequestPath (transform, currentGoal.transform, OnPathFound);
		} else {
			//if no playerobjects go to player
			goToPlayer ();
		}// check every 2 seconds distance to player
		if (secondCheck) {
			InvokeRepeating ("playerDist", 0f, 2f);
		}
	}

	//Gets all objects that enemy needs to go to
	public List<GameObject> getObjects(){
		List<GameObject> objects = new List<GameObject> ();
		objects.Add (m_base.gameObject);
		objects.AddRange (GameObject.FindGameObjectsWithTag ("PlayerCarrotField"));
		objects.AddRange (GameObject.FindGameObjectsWithTag ("PlayerTurret"));
		return objects;
	}

	public void playerDist(){
		float dist = Vector3.Distance (transform.position, m_player.position);
		if (dist < 20f) {
			CancelInvoke ();
			goToPlayer ();
		}
	}

	//Starts the function walkToPlayer every 1 second
	public void goToPlayer(){
		playerFirst = true;
		InvokeRepeating ("walkToPlayer", 0f, timeNewPath);
	}

	//Calculates path to base and walks towards
	public void goToBase(){
		playerFirst = false;
		PathRequestManager.RequestPath (transform, m_base, OnPathFound);
	}

	//Calculates path to player and walks towards
	public void walkToPlayer(){
		PathRequestManager.RequestPath (transform, m_player, OnPathFound);
	}

	public void inOutMud(bool slow){
		if (slow) {
			this.speed = mudSpeed;
		} else {
			this.speed = normalSpeed;
		}
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

		if (!pathSuccessful){
			gameObject.SetActive (false);
		}
	}

	public void stopPathfinding(){
		CancelInvoke ();
		StopCoroutine("FollowPath");
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
			/*if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}*/
			if (Vector3.Distance(transform.position,currentWaypoint) < 1f){
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];			
			}
			transform.LookAt(currentWaypoint);
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			// if enemie has hit the base, stop walking
			if (!playerFirst && baseHit) {
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
