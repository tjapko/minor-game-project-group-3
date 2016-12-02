﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  Creating and making use of the grid
/// </summary>
public class Grid : MonoBehaviour {
    
	public bool displayGridGizmos; // a boolean for visualizing the grid
	public Vector2 gridWorldSize; // the size of the whole map in coordinates
	public float nodeRadius;// radius of the nodes
	public LayerMask unwalkableMask;// a layer where all the static objects in the map are on
	Node[,] grid;  // The Grid: including walkable nodes and NOT walkable nodes.

	float nodeDiameter; // the NodeDiameter 
	int gridSizeX, gridSizeY; // The size of the whole map in NodeCoordinates 


    // creating the grid 
	void Awake() {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);// the size of the grid in nodeCoordinates relative X
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//the size og the grid in nodeCoordinates relative Z
		CreateGrid ();
	}

    // creating every wave a new grid 
    //void update()
    //{
    //    if(There is animation new round)
    //         CreateGrid();
    //}

    /// <summary>
    ///  The maxSize of the nodes, is needed when creating an Heap
    /// </summary>
	public int MaxSize{
		get {
			return gridSizeX*gridSizeY;
		}
	}

    /// <summary>
    /// The actual Grid is created.
    /// </summary>
	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY]; // An empty grid is created 
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++){// for every Node in X direction
			for (int y = 0; y < gridSizeY; y++){// for every Node in Y direction 
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); //checking whether the node is walkable
				grid [x, y] = new Node (walkable, worldPoint, x, y); //adding the Node to the grid
			}
		}
	}

    /// <summary>
    /// computing the Nodecoordinates of the neighbours with respect to the current Node  
    /// </summary>
    /// <param name="node"></param>
    /// <returns name="neighbours"></returns>
	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) {
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add (grid [checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

    /// <summary>
    /// Translates the coordinates to Nodecoordinates
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns name = "grid"></returns>
	public Node NodeFromWorldPoint(Vector3 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid [x, y];
	}

	public List<Node> path; // The path from a point to the target

    /// <summary>
    /// Visualizing the grid 
    /// </summary>
	void OnDrawGizmos() {
	Gizmos.DrawWireCube(transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));

	if (grid!= null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}