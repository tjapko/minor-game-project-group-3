using UnityEngine;
using System.Collections;

/// <summary>
/// A unit that moves from his original point to the target point 
/// </summary>
public class UnitPlayer : MonoBehaviour {

	[HideInInspector] public Transform m_base;   // the target 
	[HideInInspector] public Transform m_player;
	[HideInInspector] public float speed = 5f; // moving speed
	[HideInInspector] public bool playerFirst;
	public bool baseHit = false;

	private float distanceToPlayer;
	Vector3[] path; // The walkable path
	int targetIndex;// The index of the waypointArray. The unit moves to path[targetIndex]  

	/// <summary>
	/// on Start, requesting a path
	/// </summary>
/*	public void StartIn() {
		speed = 5f;
		baseHit = false;
	}
*/
	public void calcDistance(){
		float distToPlayer = Vector3.Distance (transform.position, m_player.position);
		float distToBase = Vector3.Distance (transform.position, m_base.position);
		if (distToBase <= distToPlayer) {
			goToBase ();
			playerFirst = false;
		} else {
			goToPlayer ();
			playerFirst = true;
		}
	}

	public void goToPlayer(){
		InvokeRepeating ("Starten", 0f, 1f);
	}

	public void goToBase(){
		PathRequestManager.RequestPath (transform.position, m_base.position, OnPathFound);
	}



	public void Starten(){
		path = null;
		targetIndex = 0;
		PathRequestManager.RequestPath (transform.position, m_player.position, OnPathFound);
		if (playerFirst && Vector3.Distance (transform.position, m_player.position) <= 8f) {
			InvokeCancel ();
		}
	}

	public void InvokeCancel(){
		Debug.Log ("Invokecancel");
		CancelInvoke ();
		PathRequestManager.RequestPath (transform.position, m_base.position, OnPathFound);
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
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	/// <summary>
	/// a coroutine for walking over the path that is given 
	/// </summary>
	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
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
