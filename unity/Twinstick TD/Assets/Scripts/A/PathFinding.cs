using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

/// <summary>
/// The algorithm for finding the path
/// </summary>
public class PathFinding : MonoBehaviour {

	PathRequestManager requestManager; // instance of requestManager
	Grid grid; // the Grid 

    /// <summary>
    /// on awake, creating a Pathrequestmanager and a grid
    /// </summary>
	void Awake() {
		requestManager = GetComponent<PathRequestManager> ();
		grid = GetComponent<Grid> ();
	}

    /// <summary>
    /// Start the coroutine Findpath 
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
	public void StartFindPath(Vector3 startPos, Vector3 targetPos){
		StartCoroutine(FindPath(startPos, targetPos));
	}

    /// <summary>
    /// Find a path using the grid 
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos){

		Vector3[] waypoints = new 	Vector3[0];	
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		if (startNode.walkable && targetNode.walkable) {

			Heap<Node> openSet = new Heap<Node> (grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node> ();
			openSet.Add (startNode);
			
			while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst ();
				closedSet.Add (currentNode);
	
				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.walkable || closedSet.Contains (neighbour)) {
						continue;
					}
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetNode);
						neighbour.parent = currentNode;
	
						if (!openSet.Contains (neighbour)) {
							openSet.Add (neighbour);
						}
					}
				}
			}
		}
		yield return null;
        
		if (pathSuccess) {
			waypoints = RetracePath (startNode, targetNode);
		}
		requestManager.FinishedProcessingPath (waypoints, pathSuccess);
	}

    /// <summary>
    /// Retrace the path back from the end Node to the start node and reverse this. Returns The final path
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns name= "waypoints"></returns>
	Vector3[] RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse(waypoints);
		return waypoints;
	}
	
    /// <summary>
    /// Simplifying the path through waypoints. Set a waypoint on the positing where the direction changes.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
	Vector3[] SimplifyPath(List<Node> path){
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
	
			for (int i = 1; i < path.Count; i++) {
				Vector2 directionNew = new Vector2 (path [i - 1].gridX - path [i].gridX, path [i - 1].gridY - path [i].gridY);
				if (directionNew != directionOld){
					waypoints.Add(path[i].worldPosition);
				}
				directionOld = directionNew;
			}
		return waypoints.ToArray();
	}

    /// <summary>
    /// Computes the distance between two nodes in the same grid 
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>

	int GetDistance(Node nodeA, Node nodeB){
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
	
		if (distX > distY) {
			return 14 * distY + 10 *(distX - distY);
		} else {
			return 14 * distX + 10 *(distY - distX);
		}
	}
}

