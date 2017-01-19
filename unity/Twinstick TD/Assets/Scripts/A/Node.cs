using UnityEngine;
using System.Collections;

/// <summary>
/// The specifications of a Node 
/// </summary>
public class Node : IHeapItem<Node>{

	public bool walkable; // Is the node walkable
	public bool placable; // Is the node placable
	public Vector3 worldPosition; // The actual worldCoordinates of the node
	public int gridX;// The place of the node in the grid in NodeCoordinates with respect to X
	public int gridY;// The place of the node in the grid in NodeCoordinates with respect to Y

    public int gCost;// Distance from starting node
	public int hCost;// Distance from target node
	public Node parent; // The previous Node from which the current node is coming 
	int heapIndex;// The index in the heaparray 

    /// <summary>
    /// The Node constructor
    /// </summary>
    /// <param name="_walkable"></param>
    /// <param name="worldPos"></param>
    /// <param name="_gridX"></param>
    /// <param name="_gridY"></param>
	public Node(bool _walkable, bool _placable, Vector3 worldPos, int _gridX, int _gridY){
		walkable = _walkable;
		placable = _placable;
		worldPosition = worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

    /// <summary>
    /// Computing the Fcost
    /// </summary>
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

    /// <summary>
    /// The get and the Set function of the HeapIndex
    /// </summary>
	public int HeapIndex{
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

    /// <summary>
    /// The CompareTo Method of a Node 
    /// </summary>
    /// <param name="nodeToCompare"></param>
    /// <returns name = "compare"></returns>
	public int CompareTo(Node nodeToCompare){
		int compare = fCost.CompareTo (nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);
		}
		return -compare;				
	}
}
