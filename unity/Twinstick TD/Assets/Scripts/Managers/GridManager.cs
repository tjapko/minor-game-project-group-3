using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager
{
	[HideInInspector] public Grid m_grid;					//Grid to be build
	[HideInInspector] public PathFinding m_pathFinding; 	//Pathfinding script
	[HideInInspector] public PathRequestManager m_request; 	//Request manager script
	[HideInInspector] public GameObject m_gridPrefab; 		//prefab of the grid
	public GameObject m_instance; 							//clone of prefab to be build in the hierarchy

	private float xGrid; 			//x length of the map
	private float yGrid; 			//y length of the map
	private float denodeRadius; 	// radius of the nodes of the grid
	private bool displayGrid; 		// display grid in scene or not

	public GridManager (GameObject gridPrefab)
	{
		this.m_gridPrefab = gridPrefab;
		this.xGrid = 150;
		this.yGrid = 150;
		this.denodeRadius = 0.5f;
		this.displayGrid = true;
		setup ();
	}

	//Sets up the grid and gets the components of the prefab
	public void setup(){
        this.m_instance = GameObject.Instantiate(m_gridPrefab) as GameObject;
        this.m_grid = m_instance.GetComponent<Grid>();
		this.m_pathFinding = m_instance.GetComponent<PathFinding>();
		this.m_request = m_instance.GetComponent<PathRequestManager>();

		m_grid.displayGridGizmos = displayGrid;
		m_grid.gridWorldSize = new Vector2 (xGrid, yGrid);
		m_grid.nodeRadius = denodeRadius;
		m_grid.unwalkableMask = LayerMask.GetMask ("Unwalkable");
		m_grid.unplacableMask = LayerMask.GetMask ("Unplacable");

	}
}