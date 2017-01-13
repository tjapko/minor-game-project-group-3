using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Creating a queue for multiple characters that are requesting a path
/// </summary>
public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();// The queue of Pathrequests 
	PathRequest currentPathRequest; // the PathRequest that is currently processing

	static PathRequestManager instance; // The pathRequestmanager 
	PathFinding pathfinding;// The pathfinding instance for using the methods of pathfinding

	bool isProcessingPath; // if there is a path being processed the boolean is true

    /// <summary>
    /// On awake, creating a PathRequestManager and an pathfinding instance
    /// </summary>
	void Awake() {
		instance = this;
		pathfinding = GetComponent<PathFinding>();
	}

    /// <summary>
    /// Add a PathRequest to the queue and process the first request in the queue  
    /// </summary>
    /// <param name="pathStart"></param>
    /// <param name="pathEnd"></param>
    /// <param name="callback"></param>
	public static void RequestPath(Transform enemy, Transform pathEnd, Action<Vector3[], bool> callback) {
		PathRequest newRequest = new PathRequest(enemy, pathEnd, callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

    /// <summary>
    /// processing the next request if there is one
    /// </summary>
	void TryProcessNext() {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.enemy, currentPathRequest.pathEnd);
		}
	}


    /// <summary>
    /// Finishing processing a path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>
	public void FinishedProcessingPath(Vector3[] path, bool success) {
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}
    
    /// <summary>
    /// The structure PathRequest
    /// </summary>
	struct PathRequest {
		//public Vector3 pathStart; // The Startposition of the path
		public Transform enemy; // gameobject player for current position
		public Transform pathEnd;// The end position of the path
		public Action<Vector3[], bool> callback;// The path in vector3 and a boolean if a good path is created

        /// <summary>
        /// The constructor PathRequest
        /// </summary>
        /// <param name="_start"></param>
        /// <param name="_end"></param>
        /// <param name="_callback"></param>
		public PathRequest(Transform _enemy, Transform _end, Action<Vector3[], bool> _callback) {
			enemy = _enemy;
			pathEnd = _end;
			callback = _callback;
		}
	}
}
