using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A unit that moves from his original point to the target point 
/// </summary>
public class UnitPlayer : MonoBehaviour {

	public GameObject m_base;  			// the baselocation 
	public Transform m_player;			// the playerlocation
	public float movementSpeed; 				// moving speed
	public float rotationSpeed = 100f;
	//public bool baseHit = false; 		// has hit the base or not
	public float timeNewPath = 2f; 		// interval between new pathcalculation to enemy
	public float mudSpeed = 1f;
	public float normalSpeed = 6f;
	public float bossSpeed = 3f;
	public float GAspeed;
	public float distanceToPlayer = 18f;
	public bool enemyType; 				// true is enemy 3, false enemy 2
	Vector3[] path; 					// The walkable path
	int targetIndex;					// The index of the waypointArray. The unit moves to path[targetIndex]  
	public GameObject currentGoal;
	private Vector3 currentWaypoint;
	public float m_scalePush = 1.5f;

	//Calculates distance to each playerObject in the scene and chooses closest as target
	public void calcDistance(bool playerCheck){
		currentGoal = null;
		enemyType = playerCheck;
		float smallestDist = 10000f;
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
		}// check every 1 seconds distance to player
		if (playerCheck) {
			InvokeRepeating ("playerDist", 0f, 1f);
		}
	}

	//Gets all objects that enemy needs to go to
	public List<GameObject> getObjects(){
		List<GameObject> objects = new List<GameObject> ();
		if (m_base.activeSelf == true) {
			objects.Add (m_base);
		}
		objects.AddRange (GameObject.FindGameObjectsWithTag ("PlayerCarrotField"));
		objects.AddRange (GameObject.FindGameObjectsWithTag ("PlayerTurret"));
		return objects;
	}

	public void playerDist(){
		float dist = Vector3.Distance (transform.position, m_player.position);
		if (dist < distanceToPlayer) {
			CancelInvoke ();
			goToPlayer ();
		}
	}

	//Starts the function walkToPlayer every 1 second
	public void goToPlayer(){
		CancelInvoke ();
		InvokeRepeating ("walkToPlayer", 0f, timeNewPath);
	}

	//Calculates path to base and walks towards
	public void goToBase(){
		if (m_base.activeInHierarchy) {
			PathRequestManager.RequestPath (transform, m_base.transform, OnPathFound);
		} else {
			goToPlayer ();
		}
	}

	//Calculates path to player and walks towards
	public void walkToPlayer(){
		PathRequestManager.RequestPath (transform, m_player, OnPathFound);
	}

	public void inOutMud(bool slow){
		if (slow) {
			this.movementSpeed = mudSpeed;
		} else {
			if (GAspeed != null){
				this.movementSpeed = GAspeed;
			}else{
				this.movementSpeed = normalSpeed;
			}
		}
	}

	public void enemyHit(Vector3 origin){
		Vector3 normalized = transform.position - m_scalePush* Vector3.Normalize (origin - transform.position);
		normalized.y = 0;
		transform.position = normalized;
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

	public void stopCoroutines(){
		StopCoroutine("FollowPath");
	}

	/// <summary>
	/// a coroutine for walking over the path that is given
	/// </summary>
	IEnumerator FollowPath() {
		if (path.Length == 0) {
			yield break;
		}
		currentWaypoint = path[0];
		//var p = Quaternion.LookRotation (path[path.Length - 1] - transform.position);
		//transform.rotation = Quaternion.RotateTowards (transform.rotation, p, 5*rotationSpeed * Time.deltaTime);

		while (true) {
			if (Vector3.Distance(currentWaypoint,transform.position) < 0.4f){
				targetIndex ++;
				//check of huidig doel er niet meer is.
				checkCurrentGoal();
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];			
			}
			float angle = Mathf.RoundToInt(Vector3.Angle (currentWaypoint - transform.position, transform.forward));
			float speed = 2 * Mathf.Pow(1.15f,(angle/2));
			var q = Quaternion.LookRotation (currentWaypoint - transform.position);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, speed * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,movementSpeed * Time.deltaTime);
			yield return null;
		}
	}

	public void checkCurrentGoal(){
		if (currentGoal == null || currentGoal.activeSelf == false) {
			calcDistance (enemyType);
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

    public void setMovementspeed(float movementspeed)
    {
        this.GAspeed = movementspeed;
		this.movementSpeed = GAspeed;
		this.rotationSpeed = 25 * GAspeed;
    }
}
